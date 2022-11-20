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
                mainCamera.transform.position = _moveToVector;
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
                break;
            case ScreenType.PLAN:
                moveToVector = planScreen.transform.position;
                break;
            case ScreenType.RANK:
                moveToVector = rankScreen.transform.position;
                break;
            case ScreenType.SHOP:
                moveToVector = shopScreen.transform.position;
                break;
            case ScreenType.INFRA:
                moveToVector = infraScreen.transform.position;
                break;
            case ScreenType.OVERALL:
                moveToVector = overallScreen.transform.position;
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