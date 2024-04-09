using Godot;
using System;

public partial class CompanyLogo : TextureRect
{

	[Export]
	public AnimationPlayer animPlayer;

	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}


	public void setStockLogo(Stock stock) {
		animPlayer.Play(stock.stockSymbol);
	}

	public void setStockLogo(string stock) {
		animPlayer.Play(stock);
	}
}
