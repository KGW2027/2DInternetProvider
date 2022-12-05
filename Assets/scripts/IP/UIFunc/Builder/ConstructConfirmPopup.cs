using System.Collections.Generic;
using IP.Control;
using IP.Objective;
using IP.Objective.Builds;
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

        public void Build()
        {
            thumbnail.texture = _buildBase.GetTexture();
            buildName.text = _buildBase.GetName();
            buildInfo.text = $"예정 완공일 : {_buildBase.GetBuildDate()}\n예상 월 소비 비용 : {_buildBase.GetBudget() / _buildBase.GetBuildDate()}k$";
        }

        public void SendData(params object[] datas)
        {
            _buildBase = (BuildBase) datas[0];
            SetDropdownMode(_buildBase.IsWire());
        }

        public void Confirm()
        {
            Debug.Log($"{_buildBase.GetName()} 건축 시작 => {GetSelected(dropdown1)}, {GetSelected(dropdown2)}");
            PopupManager.Instance.ClosePopup();
        }

        private string GetSelected(TMP_Dropdown obj)
        {
            return obj.options[obj.value].text;
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
                    bool dd1Added = false, dd2Added = false;
                    foreach (BuildBase build in GameManager.Instance.Company.GetBuilds(city))
                    {
                        if (!dd1Added && build.IsComplete() && build.IsWire())
                        {
                            dropdown1.options.Add(new TMP_Dropdown.OptionData(city.Name));
                            dd1Added = true;
                        }

                        if (!dd2Added && build.IsComplete() && (build.GetName().Equals("사무실") || build.GetName().Equals("본사")))
                        {
                            dropdown2.options.Add(new TMP_Dropdown.OptionData(city.Name));
                            dd2Added = true;
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
        }

        private string IsBuildable(City city)
        {
            // IDC는 전선이 설치 되어있어야하며, 이미 IDC가 설치되있는 경우에는 업그레이드만 가능
            // CacheServer는 IDC가 설치 되어있어야함.
            // Starlink는 IDCLarge와 CacheServer가 필요
            Company user = GameManager.Instance.Company;
            List<BuildBase> builds = user.GetBuilds(city);

            bool[] buildExists = new bool[8]; // [전선, 사무실, 서비스센터, 소형IDC, 중형IDC, 대형IDC, 캐시서버, 스타링크]
            string[] buildsName = {"", "사무실", "서비스 센터", "소형 IDC", "중형 IDC", "대형 IDC", "캐시 서버", "스타링크"};
            foreach (BuildBase build in builds)
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
    }
}