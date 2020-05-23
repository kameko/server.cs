﻿using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI.Controls
{
	public class Style : Control, IDrawingContextListener
	{
		private DrawingContext _contentContext = DrawingContext.Dummy;
		private DrawingContext ContentContext
		{
			get => _contentContext;
			set => Setter
				.SetDisposable(ref _contentContext, value)
				.Then(Initialize);
		}

		private IControl? _content;
		public IControl? Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		private Color? _background;
		public Color? Background
		{
			get => _background;
			set => Setter
				.Set(ref _background, value)
				.Then(Redraw);
		}

		private Color? _foreground;
		public Color? Foreground
		{
			get => _foreground;
			set => Setter
				.Set(ref _foreground, value)
				.Then(Redraw);
		}

		public override Cell this[Position position]
		{
			get
			{
				if (!ContentContext.Contains(position)) return Character.Empty;

				var cell = ContentContext[position];

				if (!cell.Content.HasValue) return Character.Empty;

				return cell
					.WithForeground(Foreground ?? cell.Foreground)
					.WithBackground(Background ?? cell.Background);
			}
		}

		protected override void Initialize()
		{
			using (Freeze())
			{
				ContentContext.SetLimits(MinSize, MaxSize);

				Resize(Size.Clip(MinSize, ContentContext.Size, MaxSize));
			}
		}

		private void BindContent()
		{
			ContentContext = new DrawingContext(this, Content!);
		}

		void IDrawingContextListener.OnRedraw(DrawingContext drawingContext)
		{
			Initialize();
		}

		void IDrawingContextListener.OnUpdate(DrawingContext drawingContext, Rect rect)
		{
			Update(rect);
		}
	}
}
