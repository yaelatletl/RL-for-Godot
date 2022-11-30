using Godot;
using System;
using Microsoft.ML.OnnxRuntime;
using System.Numerics.Tensors;

//Transforms common types to Godot types, might deprecate this in the future


public static class ArrayUtils 
{
	
    public static Godot.Collections.Array<string> ToArray(System.Collections.Generic.List<string> list){
        // Convert List to Array for C# to GDScript conversion (string)
        var array = new Godot.Collections.Array<string>(list);
        return array;
    }
    public static  Godot.Collections.Array<int> ToArray(System.Collections.Generic.List<int> list){
        // Convert List to Array for C# to GDScript conversion (int)
        var array = new Godot.Collections.Array<int>(list);
        return array;
    }
    public static  Godot.Collections.Array<float> ToArray(System.Collections.Generic.List<float> list){
        // Convert List to Array for C# to GDScript conversion (float)
        var array = new Godot.Collections.Array<float>(list);
        return array;
    }
    public static Godot.Collections.Array<double> ToArray(System.Collections.Generic.List<double> list){
        // Convert List to Array for C# to GDScript conversion (double)
        var array = new Godot.Collections.Array<double>(list);
        return array;
    }
    public static Godot.Collections.Array<bool> ToArray(System.Collections.Generic.List<bool> list){
        // Convert List to Array for C# to GDScript conversion (bool)
        var array = new Godot.Collections.Array<bool>(list);
        return array;
    }
    public static Godot.Collections.Array<Vector2> ToArray(System.Collections.Generic.List<Vector2> list){
        // Convert List to Array for C# to GDScript conversion (Vector2)
        var array = new Godot.Collections.Array<Vector2>(list);
        return array;
    }
    public static Godot.Collections.Array<Vector3> ToArray(System.Collections.Generic.List<Vector3> list){
        // Convert List to Array for C# to GDScript conversion (Vector3)
        var array = new Godot.Collections.Array<Vector3>(list);
        return array;
    }
    public static Godot.Collections.Array<Color> ToArray(System.Collections.Generic.List<Color> list){
        // Convert List to Array for C# to GDScript conversion (Color)
        var array = new Godot.Collections.Array<Color>(list);
        return array;
    }
    public static Godot.Collections.Array<Quat> ToArray(System.Collections.Generic.List<Quat> list){
        // Convert List to Array for C# to GDScript conversion (Quat)
        var array = new Godot.Collections.Array<Quat>(list);
        return array;
    }
    public static Godot.Collections.Array<Plane> ToArray(System.Collections.Generic.List<Plane> list){
        // Convert List to Array for C# to GDScript conversion (Plane)
        var array = new Godot.Collections.Array<Plane>(list);
        return array;
    }
    public static Godot.Collections.Array<Rect2> ToArray(System.Collections.Generic.List<Rect2> list){
        // Convert List to Array for C# to GDScript conversion (Rect2)
        var array = new Godot.Collections.Array<Rect2>(list);
        return array;
    }
    public static Godot.Collections.Array<Transform2D> ToArray(System.Collections.Generic.List<Transform2D> list){
        // Convert List to Array for C# to GDScript conversion (Transform2D)
        var array = new Godot.Collections.Array<Transform2D>(list);
        return array;
    }
    public static Godot.Collections.Array<Basis> ToArray(System.Collections.Generic.List<Basis> list){
        // Convert List to Array for C# to GDScript conversion (Basis)
        var array = new Godot.Collections.Array<Basis>(list);
        return array;
    }
    public static Godot.Collections.Array<Transform> ToArray(System.Collections.Generic.List<Transform> list){
        // Convert List to Array for C# to GDScript conversion (Transform)
        var array = new Godot.Collections.Array<Transform>(list);
        return array;
    }
    public static Godot.Collections.Array<AABB> ToArray(System.Collections.Generic.List<AABB> list){
        // Convert List to Array for C# to GDScript conversion (AABB)
        var array = new Godot.Collections.Array<AABB>(list);
        return array;
    }
    public static Godot.Collections.Array<Godot.Collections.Array> ToArray(System.Collections.Generic.List<Godot.Collections.Array> list){
        // Convert List to Array for C# to GDScript conversion (Array)
        var array = new Godot.Collections.Array<Godot.Collections.Array>(list);
        return array;
    }
    public static Godot.Collections.Array<Godot.Collections.Dictionary> ToArray(System.Collections.Generic.List<Godot.Collections.Dictionary> list){
        // Convert List to Array for C# to GDScript conversion (Dictionary)
        var array = new Godot.Collections.Array<Godot.Collections.Dictionary>(list);
        return array;
    }
}
