using System;
using System.IO;
using System.Collections.Generic;

using System.Linq;
using System.Text;
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
                    else if (__layer.name == "TileMap")
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
}
