﻿using ConsoleGUI.Space;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI
{
	public delegate void SizeLimitsChangedHandler(IDrawingContext drawingContext);

	public interface IDrawingContext
	{
		Size MinSize { get; }
		Size MaxSize { get; }

		void Redraw(IControl control);
		void Update(IControl control, in Rect rect);

		event SizeLimitsChangedHandler SizeLimitsChanged;
	}
}
