﻿using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class TextBlock : Control
	{
		private string _text = "";
		public string Text
		{
			get => _text;
			set => Setter
				.Set(ref _text, value)
				.Then(Initialize);
		}

		private Color? _color;
		public Color? Color
		{
			get => _color;
			set => Setter
				.Set(ref _color, value)
				.Then(Redraw);
		}

		public override Cell this[Position position]
		{
			get
			{
				if (Text == null) return Character.Empty;
				if (position.X >= Text.Length) return Character.Empty;
				if (position.Y >= 1) return Character.Empty;
				return new Character(Text[position.X], foreground: Color);
			}
		}

		protected override void Initialize()
		{
			Resize(new Size(Text.Length, 1));
		}
	}
}
