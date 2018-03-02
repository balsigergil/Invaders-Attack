using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : Object {

    private GameObject spawnLevel;
    private List<Enemy> enemies;

    public LevelGeneration(GameObject spawnLevel, List<Enemy> enemies)
    {
        this.spawnLevel = spawnLevel;
        this.enemies = enemies;
    }

    public void Generate(int level)
    {
        switch (level)
        {
            case 1:
                for (int width = 0; width < 8; width++)
                {
                    for (int height = 0; height < 3; height++)
                    {
                        Vector3 pos = new Vector3(spawnLevel.transform.position.x - width, spawnLevel.transform.position.y - (height * 1.6f));
                        Instantiate(enemies[0], pos, spawnLevel.transform.rotation);
                    }
                }

                break;
            case 2:
                for (int width = 0; width < 9; width++)
                {
                    for (int height = 0; height < 3; height++)
                    {
                        Vector3 pos = new Vector3(spawnLevel.transform.position.x - width, spawnLevel.transform.position.y - (height * 1.6f));
                        Instantiate(enemies[Random.Range(0, 2)], pos, spawnLevel.transform.rotation);
                    }
                }

                break;
            case 3:
                for (int width = 0; width < 10; width++)
                {
                    for (int height = 0; height < 3; height++)
                    {
                        Vector3 pos = new Vector3(spawnLevel.transform.position.x - width, spawnLevel.transform.position.y - (height * 1.6f));
                        Instantiate(enemies[Random.Range(0, 3)], pos, spawnLevel.transform.rotation);
                    }
                }

                break;

            default:
                for (int width = 0; width < 12; width++)
                {
                    for (int height = 0; height < 3; height++)
                    {
                        Vector3 pos = new Vector3(spawnLevel.transform.position.x - width, spawnLevel.transform.position.y - (height * 1.6f));
                        Enemy enemy = Instantiate(enemies[Random.Range(0, 3)], pos, spawnLevel.transform.rotation);
                        enemy.SetMovingSpeed(1 + (level - 3) * 0.05f);
                        if(level <= 13)
                            enemy.SetShootTimerRange(1.0f, 20.0f - (level - 3));
                        else
                            enemy.SetShootTimerRange(1.0f, 10.0f);
                    }
                }
                break;
        }
    }

}
