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
        public string transparentcolor { get; set; }
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
        BOTTOM_LEFT_TOP_RIGHT = 2,
        BOTTOM_RIGHT_TOP_LEFT = 3,
        TOP_LEFT_BOTTOM_RIGHT = 4,
        TOP_RIGHT_BOTTOM_LEFT = 5,
        TOP_HALF_BOTTOM_HALF = 6,
        LEFT_SIDE = 7,
        RIGHT_SIDE = 8
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
