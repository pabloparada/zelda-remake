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
        public void LoadTiledJson(string p_fileName)
        {
            Console.WriteLine(System.IO.Path.GetFullPath("../../../Room_7-7.json"));
            using (StreamReader sr = new StreamReader(System.IO.Path.GetFullPath("../../../Room_7-7.json")))
            {
                //Console.WriteLine(sr.ReadToEnd());
                // Read the stream to a string, and write the string to the console.
                RootObject __root = JsonConvert.DeserializeObject<RootObject>(sr.ReadToEnd());
                Console.WriteLine(__root.ToString());
                
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
        public List<int?> data { get; set; }
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
}
