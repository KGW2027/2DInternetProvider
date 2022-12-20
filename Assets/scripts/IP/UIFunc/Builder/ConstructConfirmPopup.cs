using System;
using System.Collections.Generic;
using IP.Control;
using IP.Objective;
using IP.Objective.Builds;
using IP.Screen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class ConstructConfirmPopup : MonoBehaviour, IUIBuilder
    {
        public RawImage thumbnail;
        public TextMeshProUGUI buildName;
        public TextMeshProUGUI buildInfo;
        public TMP_Dropdown dropdown1;
        public TMP_Dropdown dropdown2;

        private BuildBase _buildBase;

        private static readonly Vector3 WiremodeVector = new(-130.9f, -8.2f, .0f);
        private static readonly Vector3 DefaultVector = new(0, -8.2f, .0f);

        private int[] _endDate;
        private float _budget;
        private City _wireStartCity;

        public void Build()
        {
            dropdown1.value = -1;
            dropdown2.value = -1;
            thumbnail.texture = _buildBase.GetTexture();
            buildName.text = _buildBase.GetName();
            MakeBuildInfo();
        }

        public void SendData(params object[] datas)
        {
            _buildBase = (BuildBase) datas[0];
            SetDropdownMode(_buildBase.IsWire());
        }

        public void Confirm()
        {
            PopupManager.Instance.ClosePopup();
            if (_buildBase.IsWire())
            {
                // 전선 설치에서 설치할 도시가 없는 경우는 진행하지 않는다.
                if (dropdown1.options.Count == 0 || dropdown2.options.Count == 0) return;
                BuildBase construct = _buildBase.Clone();
                construct.OverrideValues(_endDate, _budget);

                City fromCity = GameManager.Instance.FindCity(GetSelected(dropdown1));
                City toCity = GameManager.Instance.FindCity(GetSelected(dropdown2));
                
                AudioManager.Instance.PlayOneShot(AudioManager.Audios.ConstructStart);
                AlertBox.New(AlertBox.AlertType.WireStart, fromCity.Name, toCity.Name, $"{construct.GetEndDate()[0]}년 {construct.GetEndDate()[1]}월");
                GameManager.Instance.Company.ConstructWire(fromCity, toCity, construct);
            }
            else
            {
                // 건물 설치에서 설치할 도시가 없는 경우는 진행하지 않는다.
                if (dropdown1.options.Count == 0) return;
                BuildBase construct = _buildBase.Clone();
                construct.OverrideValues(_endDate, _budget);
                City targetCity = GameManager.Instance.FindCity(GetSelected(dropdown1));
                    
                AudioManager.Instance.PlayOneShot(AudioManager.Audios.ConstructStart);
                AlertBox.New(AlertBox.AlertType.BuildStart, targetCity.Name, construct.GetName(), $"{construct.GetEndDate()[0]}년 {construct.GetEndDate()[1]}월");
                GameManager.Instance.Company.ConstructBuild(targetCity, construct);
            }
        }

        public void MakeBuildInfo()
        {
            if (_buildBase.IsWire())
            {
                if (dropdown1.options.Count == 0)
                {
                    buildInfo.text = "건설이 불가능합니다.";
                    return;
                }

                City fromCity = GameManager.Instance.FindCity(GetSelected(dropdown1));
                if (fromCity == null)
                {
                    buildInfo.text = "시작도시를 선택해주세요.";
                    return;
                }

                if (_wireStartCity == null || _wireStartCity != fromCity || dropdown2.options.Count == 0)
                {
                    _wireStartCity = fromCity;
                    dropdown2.options.Clear();
                    foreach (Connection conn in GameManager.Instance.GetConnections(fromCity))
                    {
                        // [전선, 사무실, 서비스센터, 소형IDC, 중형IDC, 대형IDC, 캐시서버, 스타링크]
                        bool[] buildExists = GetBuildExists(conn.EndCity);
                        if(buildExists[1]) dropdown2.options.Add(new TMP_Dropdown.OptionData(conn.EndCity.Name));
                    }

                    if (dropdown2.options.Count == 0)
                    {
                        buildInfo.text = "건설이 불가능합니다.";
                        return;
                    }
                    dropdown2.interactable = dropdown2.options.Count > 0;
                }
                City toCity = GameManager.Instance.FindCity(GetSelected(dropdown2));

                if (toCity == null)
                {
                    buildInfo.text = "목표도시를 선택해주세요.";
                    return;
                }

                if (fromCity == toCity)
                {
                    PrintBuildInfo(1, _buildBase.GetBudget()*5);
                }
                else
                {
                    float dist = Vector3.Distance(fromCity.Button.transform.position, toCity.Button.transform.position);
                    PrintBuildInfo(_buildBase.GetBuildDate() * dist, _buildBase.GetBudget() * dist);
                }

            }
            else
            {
                if (dropdown1.options.Count == 0)
                {
                    buildInfo.text = "건설이 불가능합니다.";
                    return;
                }

                float budget = _buildBase.GetBudget();
                float date = _buildBase.GetBuildDate();
                
                City selectedCity = GameManager.Instance.FindCity(GetSelected(dropdown1));
                if (selectedCity == null)
                {
                    buildInfo.text = "도시를 선택해주세요.";
                    return;
                }

                // [전선, 사무실, 서비스센터, 소형IDC, 중형IDC, 대형IDC, 캐시서버, 스타링크]
                bool[] cityInfo = GetBuildExists(selectedCity);
                if (cityInfo[5]) // 대형 IDC가 있을 경우 캐시 서버 건설 기간을 1/3, 비용을 2/5로 줄여준다.
                {
                    if (_buildBase.GetName().Equals("캐시 서버"))
                    {
                        date *= 0.33f;
                        budget *= 0.4f;
                    }
                }
                else if (cityInfo[4]) // 중형 IDC가 있을 경우 캐시 서버 건설 기간을 2/3, 비용을 3/5로 줄여주고, 대형 IDC의 건설 기간을 12개월 단축 시킨다.
                {
                    if (_buildBase.GetName().Equals("캐시 서버"))
                    {
                        date *= 0.66f;
                        budget *= 0.6f;
                    }
                    else if (_buildBase.GetName().Equals("대형 IDC"))
                    {
                        budget -= (budget / date) * 12;
                        date -= 12;
                    }
                }
                else if (cityInfo[3]) // 소형 IDC가 있을 경우, 중형 및 대형 IDC 건설 기간을 6개월 단축 시킨다.
                {
                    if (_buildBase.GetName().Equals("중형 IDC") || _buildBase.GetName().Equals("대형 IDC"))
                    {
                        budget -= (budget / date) * 6;
                        date -= 6;
                    }
                }
                
                PrintBuildInfo(date, budget);
            }
        }

        private void PrintBuildInfo(float date, float budget)
        {
            int completeDate = (int) Math.Round(date);
            int year = completeDate / 12, month = completeDate % 12;
            int[] endDate = AppBarManager.Instance.GetDate();
            endDate[0] += year;
            endDate[1] += month;
            if (endDate[1] > 12)
            {
                endDate[0]++;
                endDate[1] -= 12;
            }

            _endDate = endDate;
            _budget = budget / completeDate;
            buildInfo.text = $"예정 완공일 : {endDate[0]:00}년 {endDate[1]:00}월\n예상 월 소비 비용 : {budget/completeDate:F2}k$";
        }

        private string GetSelected(TMP_Dropdown obj)
        {
            if (obj.value < 0 || obj.options[obj.value].text is null or "") return null;
            return obj.options[obj.value].text.Split(" ")[0];
        }

        private void SetDropdownMode(bool isWire)
        {
            if (isWire)
            {
                // Dropdown1에는 이미 전선이 설치된 도시들 목록을 표시
                // Dropdown2에는 Office가 설치된 도시들 목록 표시
                dropdown2.options.Clear();
                dropdown1.options.Clear();
                GameManager.Instance.Company.GetConnectedCities().ForEach(city =>
                {
                    bool dd1Added = false;
                    if (GameManager.Instance.Company.GetBuilds(city) != null)
                    {
                        foreach (BuildBase build in GameManager.Instance.Company.GetBuilds(city))
                        {
                            if (!dd1Added && build.IsComplete() && build.IsWire())
                            {
                                dropdown1.options.Add(new TMP_Dropdown.OptionData(city.Name));
                                dd1Added = true;
                            }
                        }
                    }
                });
                
                dropdown1.transform.SetLocalPositionAndRotation(WiremodeVector, Quaternion.identity);
                dropdown2.gameObject.SetActive(true);
            }
            else
            {
                dropdown1.options.Clear();
                // 사무실은 건물이 있는 도시에서 Connection이 있는 모든 도시에 설치 가능
                if (_buildBase.GetName().Equals("사무실"))
                {
                    List<City> added = new List<City>();
                    foreach (City city in GameManager.Instance.Company.GetConnectedCities())
                    {
                        List<Connection> connections = GameManager.Instance.GetConnections(city);
                        connections.ForEach(conn =>
                        {
                            if (GameManager.Instance.Company.GetBuilds(conn.EndCity) == null && !added.Contains(conn.EndCity))
                            {
                                added.Add(conn.EndCity);
                                dropdown1.options.Add(new TMP_Dropdown.OptionData(conn.EndCity.Name));
                            }
                        });
                    }
                }
                else
                {
                    GameManager.Instance.Company.GetConnectedCities().ForEach(city =>
                    {
                        string buildable = IsBuildable(city);
                        if (buildable != null) dropdown1.options.Add(new TMP_Dropdown.OptionData(buildable));
                    });
                }

                dropdown2.gameObject.SetActive(false);
                dropdown1.transform.SetLocalPositionAndRotation(DefaultVector, Quaternion.identity);
            }

            // 설치 가능한 도시가 없다면 비활성화
            dropdown1.interactable = dropdown1.options.Count > 0;
            dropdown2.interactable = dropdown2.options.Count > 0;
        }

        private string IsBuildable(City city)
        {
            // IDC는 전선이 설치 되어있어야하며, 이미 IDC가 설치되있는 경우에는 업그레이드만 가능
            // CacheServer는 IDC가 설치 되어있어야함.
            // Starlink는 IDCLarge와 CacheServer가 필요
            bool[] buildExists = GetBuildExists(city);
            
            bool canBuild = false;
            string cityName = city.Name;
            switch (_buildBase.GetName())
            {
                case "서비스 센터":
                    canBuild = buildExists[1] && !buildExists[2];
                    break;
                case "소형 IDC":
                    canBuild = buildExists[0] && buildExists[1] && !buildExists[3] && !buildExists[4] && !buildExists[5];
                    cityName += " [New]";
                    break;
                case "중형 IDC":
                    canBuild = buildExists[0] && buildExists[1] && !buildExists[4] && !buildExists[5];
                    cityName += buildExists[3] ? " [Upgrade]" : "[New]";
                    break;
                case "대형 IDC":
                    canBuild = buildExists[0] && buildExists[1] && !buildExists[5];
                    cityName += buildExists[3] || buildExists[4] ? " [Upgrade]" : "[New]";
                    break;
                case "캐시 서버":
                    canBuild = buildExists[3] || buildExists[4] || buildExists[5];
                    break;
                case "스타링크":
                    canBuild = buildExists[5] && buildExists[6];
                    break;
            }

            if (canBuild) return cityName;
            return null;
        }

        private bool[] GetBuildExists(City city)
        {
            
            bool[] buildExists = new bool[8]; // [전선, 사무실, 서비스센터, 소형IDC, 중형IDC, 대형IDC, 캐시서버, 스타링크]
            string[] buildsName = {"", "사무실", "서비스 센터", "소형 IDC", "중형 IDC", "대형 IDC", "캐시 서버", "스타링크"};
            if (GameManager.Instance.Company.GetBuilds(city) == null) return buildExists;
            foreach (BuildBase build in GameManager.Instance.Company.GetBuilds(city))
            {
                if (build.IsWire())
                {
                    buildExists[0] = true;
                    continue;
                }

                if (build.GetName().Equals("본사"))
                {
                    buildExists[1] = true;
                    continue;
                }
                
                for (int key = 1; key < 8; key++)
                {
                    if (build.GetName().Equals(buildsName[key])) buildExists[key] = true;
                }
            }

            return buildExists;
        }
    }
}