﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Data
{
	public readonly struct Character
	{
		public readonly char? Content;

		public readonly Color? Foreground;
		public readonly Color? Background;

		public Character(char? content, Color? foreground = null, Color? background = null)
		{
			Content = content;
			Foreground = foreground;
			Background = background;
		}

		public Character(in Color background)
		{
			Content = null;
			Foreground = null;
			Background = background;
		}

		public Character WithContent(char? content) => new Character(content, Foreground, Background);
		public Character WithForeground(in Color? foreground) => new Character(Content, foreground, Background);
		public Character WithBackground(in Color? background) => new Character(Content, Foreground, background);

		public static Character Empty => new Character();

		public static bool operator==(in Character lhs, in Character rhs)
		{
			return lhs.Content == rhs.Content &&
				   lhs.Foreground == rhs.Foreground &&
				   lhs.Background == rhs.Background;
		}

		public static bool operator !=(in Character lhs, in Character rhs) => !(lhs == rhs);

		public override bool Equals(object? obj)
		{
			return obj is Character character && this == character;
		}

		public override int GetHashCode()
		{
			var hashCode = -1661473088;
			hashCode = hashCode * -1521134295 + EqualityComparer<char?>.Default.GetHashCode(Content!);
			hashCode = hashCode * -1521134295 + EqualityComparer<Color?>.Default.GetHashCode(Foreground!);
			hashCode = hashCode * -1521134295 + EqualityComparer<Color?>.Default.GetHashCode(Background!);
			return hashCode;
		}
	}
}
