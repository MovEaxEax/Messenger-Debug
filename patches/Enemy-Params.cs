using System;
public class TurtleManParams : EnemySpawnParams
{
	public TurtleManParams()
	{
	}

	public EEnemyStartingDirection StartDirection
	{
		get
		{
			return this.startDirection;
		}
	}

	public float Speed
	{
		get
		{
			return this.speed;
		}
	}

	public bool SyncTransformOnMove
	{
		get
		{
			return this.syncTransformOnMove;
		}
	}

	public EEnemyStartingDirection startDirection = EEnemyStartingDirection.TOWARDS_PLAYER;
	public float speed;
	public bool syncTransformOnMove = true;
}





using System;
using UnityEngine;
public class BouncingDogoParams : EnemySpawnParams
{
	public BouncingDogoParams()
	{
	}

	public EEnemyStartingDirection StartDirection
	{
		get
		{
			return this.startDirection;
		}
	}

	public EEnemyStartingDirection startDirection = EEnemyStartingDirection.TOWARDS_PLAYER;
	public float speed = 1f;
	public float jumpHeight = 4f;
	public float timeToJumpApex = 0.4f;
	public bool syncTransformOnMove = true;
}





using System;
using UnityEngine;
public class TurtleManRangedParams : EnemySpawnParams
{
	public TurtleManRangedParams()
	{
	}

	public EEnemyStartingDirection StartLookDirection
	{
		get
		{
			return this.startLookDirection;
		}
	}

	public bool AlwaysFacePlayer
	{
		get
		{
			return this.alwaysFacePlayer;
		}
	}

	public int ShotsPerWave
	{
		get
		{
			return this.shotsPerWave;
		}
	}

	public float DelayBetweenWaves
	{
		get
		{
			return this.delayBetweenWaves;
		}
	}

	public float DelayBetweenShots
	{
		get
		{
			return this.delayBetweenShots;
		}
	}

	public float FirstShotCD
	{
		get
		{
			return this.firstShotCD;
		}
	}

	public int EnemyDamage
	{
		get
		{
			return this.enemyDamage;
		}
	}

	public int ProjectileDamage
	{
		get
		{
			return this.projectileDamage;
		}
	}

	public float ProjectileSpeed
	{
		get
		{
			return this.projectileSpeed;
		}
	}

	public EEnemyStartingDirection startLookDirection = EEnemyStartingDirection.TOWARDS_PLAYER;
	public bool alwaysFacePlayer;
	public int shotsPerWave;
	public float delayBetweenWaves;
	public float delayBetweenShots;
	public float firstShotCD;
	public int enemyDamage;
	public int projectileDamage;
	public float projectileSpeed;
}





using System;
using UnityEngine;
public class SkeloutonParams : EnemySpawnParams
{
	public SkeloutonParams()
	{
	}

	public EEnemyStartingDirection StartDirection
	{
		get
		{
			return this.startDirection;
		}
	}

	public float Speed
	{
		get
		{
			return this.speed;
		}
	}

	public float SpeedBoostMultiplier
	{
		get
		{
			return this.speedBoostMultiplier;
		}
	}

	public float SpeedBoostRangeY
	{
		get
		{
			return this.speedBoostRangeY;
		}
	}

	public float ActivateRange
	{
		get
		{
			return this.activateRange;
		}
	}

	public EEnemyStartingDirection startDirection = EEnemyStartingDirection.TOWARDS_PLAYER;
	public float speed;
	public float speedBoostMultiplier;
	public float speedBoostRangeY;
	public float activateRange;
}





using System;
using UnityEngine;
public class BirdyParams : EnemySpawnParams
{
	public BirdyParams()
	{
	}

	public float AccelerationX
	{
		get
		{
			return this.accelerationX;
		}
	}

	public float MaxSpeed
	{
		get
		{
			return this.maxSpeed;
		}
	}

	public float accelerationX = 0.3f;
	public float maxSpeed = 3f;
	public EnemyActivationZone activationZone;
}
























