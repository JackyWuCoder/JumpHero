using Godot;
using System;

namespace JumpHero
{
	public partial class PlayerColorManager : Node
	{
		private Player _player;
		private Sprite2D _sprite;
		private Color _finalColor;
		public override void _Ready()
		{
			_player = GetOwner<Player>();
			_sprite = GetParent<Sprite2D>();
			_player.Connect(Player.SignalName.OnChargeChange, Callable.From((float percent) => GlowSprite(percent)));
			_finalColor = _sprite.Modulate * 2.5f;
		}

		private void GlowSprite(float percent)
		{
			if (percent > 0) 
			{
				_sprite.Modulate = (_finalColor - Player.DEFAULT_PLAYER_COLOR) * percent + Player.DEFAULT_PLAYER_COLOR;
				_player.UpdateColor(_sprite.Modulate);
			}
			else
			{
				_player.UpdateColor(Player.DEFAULT_PLAYER_COLOR);
				// Create transition back to default color
				Tween tween = CreateTween();
				tween.TweenProperty(_sprite, nameof(_sprite.Modulate).ToLower(), Player.DEFAULT_PLAYER_COLOR, 0.3f);
				tween.Play();
			}
		}
	}
}
