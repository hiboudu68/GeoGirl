using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Demonstrations;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AIAgent : Agent
{
    private GameGrid grid;
    private float lastX;

    void Start()
    {
        grid = FindAnyObjectByType<GameGrid>();
        Player.OnWin += OnWin;
        Player.OnDie += OnDie;
        Application.targetFrameRate = -1;
    }
    void FixedUpdate()
    {
        RequestDecision();
    }
    private void OnWin()
    {
        SetReward(200f);
        EndEpisode();
    }
    private void OnDie()
    {
        VictoryTrigger victory = FindAnyObjectByType<VictoryTrigger>();
        if (victory != null)
            SetReward(Player.Instance.transform.position.x - victory.transform.position.x);
        Debug.Log("DIE");
        EndEpisode();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        if (Player.Instance == null)
        {
            Destroy(this);
            return;
        }

        Vector3 playerPos = Player.Instance.transform.position;
        sensor.AddObservation(playerPos);
        foreach (BaseObject obj in grid.Objects)
        {
            Vector3 objPos = obj.transform.position;
            if (objPos.x > playerPos.x && objPos.x - playerPos.x < 10f && obj.GetComponentInChildren<DeadZoneProperty>() != null)
            {
                sensor.AddObservation(objPos - playerPos);
            }
        }

        sensor.AddObservation(Player.Instance.GetComponent<Rigidbody2D>().linearVelocity.y);
        sensor.AddObservation(Player.Instance.IsGrounded);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float curX = Player.Instance.transform.position.x;
        if (actions.DiscreteActions[0] == 1)
        {
            switch (Player.Instance.currentMode)
            {
                case PlayerJumpHandler jumper:
                    if (Player.Instance.IsGrounded)
                        jumper.Jump();
                    break;
                case PlayerShip ship:
                    ship.Lift();
                    break;
                case PlayerBall ball:
                    ball.Reverse();
                    break;
            }
        }
        else
        {
            AddReward((curX - lastX) * 1f);
        }

        lastX = curX;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        actionsOut.DiscreteActions.Array[0] = (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) ? 1 : 0;
    }
    public override void OnEpisodeBegin()
    {
        Player.Instance.Restart();
        lastX = Player.Instance.transform.position.x;
    }
}
