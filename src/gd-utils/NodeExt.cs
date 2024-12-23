﻿using Godot;

namespace Godot25.Presentation.Utils
{
    public static class NodeExt
    {
        public static void ClearChildren(this Node node)
        {
            foreach (Node item in node.GetChildren())
            {
                item.QueueFree();
            }
        }
    }
}
