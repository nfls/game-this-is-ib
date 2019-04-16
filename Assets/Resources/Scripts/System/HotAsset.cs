using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class HotAsset<T> where T : Object {

	public bool required;
	public readonly string type;
	public string identifier;

	public abstract T Source { get; }

	public HotAsset(string type) {
		this.type = type;
	}

	public static implicit operator bool(HotAsset<T> asset) => asset.required;
}

[Serializable]
public class ItemAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetItem(identifier);
	public ItemAsset() : base(ResourcesManager.ITEM_PREFIX) { }

	public static implicit operator ItemAsset(string identifier) => new ItemAsset { required = true, identifier = identifier };
}

[Serializable]
public class IBSpriteAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetIBSprite(identifier);
	public IBSpriteAsset() : base(ResourcesManager.IB_SPRITE_PREFIX) { }
	
	public static implicit operator IBSpriteAsset(string identifier) => new IBSpriteAsset { required = true, identifier = identifier };
}

[Serializable]
public class ProjectileAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetProjectile(identifier);
	public ProjectileAsset() : base(ResourcesManager.PROJECTILE_PREFIX) { }

	public T Get<T>() where T : ProjectileController => ProjectileManager.Get(identifier) as T;
	
	public static implicit operator ProjectileAsset(string identifier) => new ProjectileAsset { required = true, identifier = identifier };
}

[Serializable]
public class ParticleAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetParticle(identifier);
	public ParticleAsset() : base(ResourcesManager.PARTICLE_PREFIX) { }
	
	public T Get<T>() where T : ParticleController => ParticleManager.Get(identifier) as T;
	
	public static implicit operator ParticleAsset(string identifier) => new ParticleAsset { required = true, identifier = identifier };
}

[Serializable]
public class BodyAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetBody(identifier);
	public BodyAsset() : base(ResourcesManager.BODY_PREFIX) { }
	
	public static implicit operator BodyAsset(string identifier) => new BodyAsset { required = true, identifier = identifier };
}

[Serializable]
public class EyeAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetEye(identifier);
	public EyeAsset() : base(ResourcesManager.EYE_PREFIX) { }
	
	public static implicit operator EyeAsset(string identifier) => new EyeAsset { required = true, identifier = identifier };
}

[Serializable]
public class EnemyAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetEnemy(identifier);
	public EnemyAsset() : base(ResourcesManager.ENEMY_PREFIX) { }
	
	public static implicit operator EnemyAsset(string identifier) => new EnemyAsset { required = true, identifier = identifier };
}

[Serializable]
public class BossAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetBoss(identifier);
	public BossAsset() : base(ResourcesManager.BOSS_PREFIX) { }
	
	public static implicit operator BossAsset(string identifier) => new BossAsset { required = true, identifier = identifier };
}

[Serializable]
public class UIAsset : HotAsset<GameObject> {
	public override GameObject Source => ResourcesManager.GetUI(identifier);
	public UIAsset() : base(ResourcesManager.UI_PREFIX) { }
	
	public static implicit operator UIAsset(string identifier) => new UIAsset { required = true, identifier = identifier };
}

[Serializable]
public class SpriteAsset : HotAsset<Sprite> {
	public override Sprite Source => ResourcesManager.GetSprite(identifier);
	public SpriteAsset() : base(ResourcesManager.SPRITE_PREFIX) { }
	
	public static implicit operator SpriteAsset(string identifier) => new SpriteAsset { required = true, identifier = identifier };
}

[Serializable]
public class AudioAsset : HotAsset<AudioClip> {
	public override AudioClip Source => ResourcesManager.GetAudio(identifier);
	public AudioAsset() : base(ResourcesManager.AUDIO_PREFIX) { }
	
	public static implicit operator AudioAsset(string identifier) => new AudioAsset { required = true, identifier = identifier };
}