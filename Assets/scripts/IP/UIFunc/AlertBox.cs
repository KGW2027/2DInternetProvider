using System.Collections.Generic;
using IP.UIFunc.Builder;
using MEC;
using UnityEngine;

namespace IP.UIFunc
{
    public class AlertBox : MonoBehaviour
    {
        private static AlertBox _instance;

        public enum AlertType
        {
            BuildStart,
            BuildEnd,
            WireStart,
            WireEnd,
            Loan,
            LoanEnd
        }

        private const float DisposeY = 215.4f;
        private const float BoxHeight = 88.4f;
        private const float SpawnY = -119.2f;

        public GameObject alertBox;
        public float alertDisposeTime = 3.0f;

        private List<GameObject> _alerts;

        void Start()
        {
            _instance = this;
            _alerts = new List<GameObject>();
            Timing.RunCoroutine(AlertFlow());
        }

        public static void New(AlertType type, params string[] args)
        {
            _instance.AddAlert(type, args);
        }


        private void AddAlert(AlertType type, params string[] args)
        {
            ForceUp();
            GameObject box = Instantiate(alertBox, transform);
            box.transform.SetLocalPositionAndRotation(new(0, SpawnY, 0), Quaternion.identity);
            _alerts.Add(box);
            AlertPopup popup = box.GetComponent<AlertPopup>();

            popup.SendData(GetMainText(type, args), GetSubText(type, args));
            popup.Build();
        }

        private string GetMainText(AlertType type, params string[] args)
        {
            switch (type)
            {
                case AlertType.BuildStart:
                    return $"[건설 시작] {args[0]}에 {args[1]} 건설 시작";
                case AlertType.BuildEnd:
                    return $"[건설 완료] {args[0]}에 {args[1]} 완공";
                case AlertType.WireStart:
                    return $"[전선 건설] {args[0]}부터 {args[1]}";
                case AlertType.WireEnd:
                    return $"[전선 연결] {args[0]}부터 {args[1]}";
                case AlertType.Loan:
                    return $"[대출 시작] {args[0]}에서 {args[1]}대출";
                case AlertType.LoanEnd:
                    return $"[대출 만료] {args[0]} 상환";
            }

            return "NULL";
        }

        private string GetSubText(AlertType type, params string[] args)
        {
            switch (type)
            {
                case AlertType.BuildStart:
                    return $"예상 완공 시기 : {args[2]}";
                case AlertType.BuildEnd:
                    return $"예상 월 유지비용 : {args[2]}$";
                case AlertType.WireStart:
                    return $"예상 완공 시기 : {args[2]}";
                case AlertType.WireEnd:
                    return $"예상 월 유지비용 : {args[2]}$";
                case AlertType.Loan:
                    return $"예상 월 이자 : {args[2]}";
                case AlertType.LoanEnd:
                    return "";

            }

            return "NULL";
        }

        private void ForceUp()
        {
            int removed = 0;
            foreach (GameObject go in _alerts)
                if (MoveBox(go, BoxHeight + 5f))
                    removed++;
            RemoveQueue(removed);
        }

        private void RemoveQueue(int amount)
        {
            while (amount-- > 0 && _alerts.Count > 0) _alerts.RemoveAt(0);
        }

        private bool MoveBox(GameObject go, float amount)
        {
            var pos = go.transform.localPosition;
            pos.y += amount;
            if (pos.y >= DisposeY)
            {
                Destroy(go);
                return true;
            }

            go.transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
            return false;
        }


        IEnumerator<float> AlertFlow()
        {
            float movedOneTime = (DisposeY - SpawnY) / (alertDisposeTime / 0.1f);

            while (true)
            {
                int removed = 0;
                foreach (GameObject go in _alerts)
                    if (MoveBox(go, movedOneTime)) removed++;
                
                RemoveQueue(removed);
                yield return Timing.WaitForSeconds(0.1f);
            }

        }
    }
}