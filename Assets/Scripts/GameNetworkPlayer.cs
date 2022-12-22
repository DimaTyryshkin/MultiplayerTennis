using MultiplayerTennis.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GameNetworkPlayer : NetworkBehaviour
{
    Camera gameCamera;
    TennisRacquetMovement tennisRacquet;

    protected Vector2 Point => gameCamera.ScreenPointToRay(UnityEngine.Input.mousePosition).origin;
    public event UnityAction PlayerReady;

    void Start()
    {
        if (!isServer && isLocalPlayer)
            CmdClientReady();
    }

    void Update()
    {
        if (!tennisRacquet)
            return;

        if (!isLocalPlayer)
            return;

        Vector2 input = ReadLocalInput();
        CmdInput(input);
    }

    public void Init(Camera gameCamera, TennisRacquetMovement tennisRacquet)
    {
        Assert.IsNotNull(gameCamera);
        Assert.IsNotNull(tennisRacquet);

        this.gameCamera = gameCamera;
        this.tennisRacquet = tennisRacquet;
    }

    public void KikClient()
    {
        GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
    }

    Vector2 ReadLocalInput()
    {
        Vector2 pos = tennisRacquet.transform.position;

        Vector2 dirToTarget = Point - pos;
        Vector2 input = Vector3.Project(dirToTarget, tennisRacquet.Right);
        input = pos + input;
        return input;
    }

    [Command]
    void CmdInput(Vector2 input)
    {
        if (isServer)
            tennisRacquet.Input(input);
    }

    [Command]
    void CmdClientReady()
    {
        if (isServer && !isLocalPlayer)
            PlayerReady?.Invoke();
    }
}