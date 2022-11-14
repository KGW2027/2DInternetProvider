using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public enum ScreenType
    {
        MAP, INFRA, PLAN, SHOP, BANK, RANK, OVERALL
    }
        
    public GameObject mapScreen;
    public GameObject infraScreen;
    public GameObject planScreen;
    public GameObject shopScreen;
    public GameObject bankScreen;
    public GameObject rankScreen;
    public GameObject overallScreen;
    public GameObject mainCamera;
    public int cameraMoveSpeed = 10;

    public GameObject mapUI;

    private bool _isLerping;
    private Vector3 _moveToVector;

    // Start is called before the first frame update
    void Start()
    {
        _isLerping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isLerping)
        {
            if (Input.GetKeyDown(KeyCode.A)) MoveCamara(ScreenType.MAP);
            else if(Input.GetKeyDown(KeyCode.B)) MoveCamara(ScreenType.INFRA);
            else if(Input.GetKeyDown(KeyCode.C)) MoveCamara(ScreenType.BANK);
            else if(Input.GetKeyDown(KeyCode.D)) MoveCamara(ScreenType.PLAN);
            else if(Input.GetKeyDown(KeyCode.E)) MoveCamara(ScreenType.RANK);
            else if(Input.GetKeyDown(KeyCode.F)) MoveCamara(ScreenType.SHOP);
            else if(Input.GetKeyDown(KeyCode.G)) MoveCamara(ScreenType.OVERALL);
        }

        if (_isLerping)
        {
            Vector3 cameraPos = mainCamera.transform.position;
            mainCamera.transform.position = Vector3.Lerp(cameraPos, _moveToVector, Time.deltaTime * cameraMoveSpeed);

            if (Vector3.Distance(cameraPos, _moveToVector) < .1f)
            {
                Vector3 isMap = mapScreen.transform.position;
                isMap.z = -10.0f;
                if(_moveToVector == isMap)
                    mapUI.SetActive(true);
                _isLerping = false;
            }
        }
    }

    public void MoveCamara(ScreenType type)
    {
        Vector3 moveToVector;
        switch (type)
        {
            case ScreenType.BANK: 
                moveToVector = bankScreen.transform.position;
                mapUI.SetActive(false);
                break;
            case ScreenType.PLAN:
                moveToVector = planScreen.transform.position;
                mapUI.SetActive(false);
                break;
            case ScreenType.RANK:
                moveToVector = rankScreen.transform.position;
                mapUI.SetActive(false);
                break;
            case ScreenType.SHOP:
                moveToVector = shopScreen.transform.position;
                mapUI.SetActive(false);
                break;
            case ScreenType.INFRA:
                moveToVector = infraScreen.transform.position;
                mapUI.SetActive(false);
                break;
            case ScreenType.OVERALL:
                moveToVector = overallScreen.transform.position;
                mapUI.SetActive(false);
                break;
            default:
                moveToVector = mapScreen.transform.position;
                break;
        }

        moveToVector.z = -10;
        _moveToVector = moveToVector;
        _isLerping = true;
    }
        
}