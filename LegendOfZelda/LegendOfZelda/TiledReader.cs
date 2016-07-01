using System;
using System.IO;
using System.Collections.Generic;

using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace LegendOfZelda
{
    public class TiledReader
    {
        public RootObject LoadTiledJson(string p_fileName)
        {
            Console.WriteLine(System.IO.Path.GetFullPath("../../../TileMaps/" + p_fileName + ".json"));
            using (StreamReader sr = new StreamReader(System.IO.Path.GetFullPath("../../../TileMaps/" + p_fileName + ".json")))
            {
                RootObject __rootObj = JsonConvert.DeserializeObject<RootObject>(sr.ReadToEnd());
                foreach (Layer __layer in __rootObj.layers)
                {
                    if (__layer.name == "CollisionMask")
                    {
                        for (int i = 0; i < __layer.data.Count; i++)
                            __layer.data[i] -= 1025;
                    }
                    else if (__layer.name.StartsWith("TileMap"))
                    {
                        for (int i = 0; i < __layer.data.Count; i++)
                            __layer.data[i] -= 1;
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
    }

    public class Propertytypes
    {
        public string TargetMap { get; set; }
        public string TargetPositionX { get; set; }
        public string TargetPositionY { get; set; }
        public string TransitionType { get; set; }
        public string Name { get; set; }
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
        HALF_RIGHT = 9
    }

    public class RootObjectUtil
    {
        public static Layer GetLayerByName(RootObject p_root, string p_layerName)
        {
            foreach (Layer __layer in p_root.layers)
                if (__layer.name == p_layerName)
                    return __layer;
            return null;
        }

        public static Tileset FindTilesetByName(RootObject p_root, string p_tilesetName)
        {
            return p_root.tilesets.FindAll(p => p.name == p_tilesetName).First();
        }
    }
}
