using UnityEngine;

public class PlayerDeadControl : PlayerController
{
    private float spawnTime;
    private Player player;

    protected override void InitControl()
    {
        player = GetComponentInParent<Player>();
        spawnTime = Time.realtimeSinceStartup;
        IsActive = false;
    }
    // Update is called once per frame
    void Update()
    {
        GetComponentInParent<Rigidbody2D>().gravityScale = 0;
        if (Time.realtimeSinceStartup - spawnTime > 2f)
        {
            player.Restart();
        }
    }
}
