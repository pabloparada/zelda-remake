﻿using System.IO;
using System.Collections.Generic;

using System.Linq;
using Newtonsoft.Json;

namespace LegendOfZelda
{
    public class TiledReader
    {
        public RootObject LoadTiledJson(string p_fileName)
        {
            using (var __sr = new StreamReader(Path.GetFullPath("../../../TileMaps/" + p_fileName + ".json")))
            {
                var __rootObj = JsonConvert.DeserializeObject<RootObject>(__sr.ReadToEnd());
                foreach (var __layer in __rootObj.layers)
                {
                    if (__layer.name == "CollisionMask")
                    {
                        for (var __i = 0; __i < __layer.data.Count; __i++)
                            __layer.data[__i] -= 1025;
                    }
                    else if (__layer.name.StartsWith("TileMap"))
                    {
                        for (var __i = 0; __i < __layer.data.Count; __i++)
                            __layer.data[__i] -= 1;
                    }
                }
                return __rootObj;
            }
        }
    }

    public class Properties
    {
        public string TargetMap { get; set; }
        public int TargetPositionX { get; set; }
        public int TargetPositionY { get; set; }
        public int TransitionType { get; set; }
        public string Name { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public int KeyType { get; set; }
        public int DoorType { get; set; }
        public int DoorSide { get; set; }
        public int TileToOpen { get; set; }
        public bool CloseOnEnter { get; set; }
    }

    public class Propertytypes
    {
        public string TargetMap { get; set; }
        public string TargetPositionX { get; set; }
        public string TargetPositionY { get; set; }
        public string TransitionType { get; set; }
        public string Name { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
    }

    public class Object
    {
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Properties properties { get; set; }
        public Propertytypes propertytypes { get; set; }
        public int rotation { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Layer
    {
        public int height { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public int offsetx { get; set; }
        public int offsety { get; set; }
        public double opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public List<int> data { get; set; }
        public string draworder { get; set; }
        public List<Object> objects { get; set; }
    }

    public class Tileset
    {
        public int columns { get; set; }
        public int firstgid { get; set; }
        public string image { get; set; }
        public int imageheight { get; set; }
        public int imagewidth { get; set; }
        public int margin { get; set; }
        public string name { get; set; }
        public int spacing { get; set; }
        public int tilecount { get; set; }
        public int tileheight { get; set; }
        public int tilewidth { get; set; }
    }

    public class RootObject
    {
        public int height { get; set; }
        public List<Layer> layers { get; set; }
        public int nextobjectid { get; set; }
        public string orientation { get; set; }
        public string renderorder { get; set; }
        public int tileheight { get; set; }
        public List<Tileset> tilesets { get; set; }
        public int tilewidth { get; set; }
        public int version { get; set; }
        public int width { get; set; }
    }
    public enum CollisionMask
    {
        NONE = 0,
        FULL = 1, 
        DIAGONAL_TOP_LEFT = 2,
        DIAGONAL_TOP_RIGHT = 3,
        DIAGONAL_BOTTOM_LEFT = 4,
        DIAGONAL_BOTTOM_RIGHT = 5,
        HALF_TOP = 6,
        HALF_BOTTOM = 7,
        HALF_LEFT = 8,
        HALF_RIGHT = 9,
        WATER = 10
    }

    public class RootObjectUtil
    {
        public static Layer GetLayerByName(RootObject p_root, string p_layerName)
        {
            return p_root.layers.FirstOrDefault(p_layer => p_layer.name == p_layerName);
        }

        public static Tileset FindTilesetByName(RootObject p_root, string p_tilesetName)
        {
            return p_root.tilesets.FindAll(p => p.name == p_tilesetName).First();
        }
    }
}
