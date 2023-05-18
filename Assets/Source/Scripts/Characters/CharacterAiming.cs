using UnityEngine;

public class CharacterAiming
{
	private CharacterModel _model;
    
	public CharacterAiming(CharacterModel model)
	{
		_model = model;
	}

	public void Aim(Vector3 target)
	{
		target.y = _model.transform.position.y;
		
		_model.transform.LookAt(target);
	}
}