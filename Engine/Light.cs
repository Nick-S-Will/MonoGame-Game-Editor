﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Engine;

internal class Light
{
    public Light() { }

    public Vector3 Position { get; set; }
    public Color Color { get; set; }
}
