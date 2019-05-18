using System.ComponentModel;

namespace GarbageCollector.Domain
{
    public enum Material
    {
        //Картон,Бумага,Пластик,Металл,Стекло,Батарейки,Одежда
        None = 0,
        [Description("Пластик")]
        Plastic = 1,
        [Description("Картон")]
        Carton = 2,
        [Description("Бумага")]
        Paper = 3,
        [Description("Металл")]
        Metal = 4,
        [Description("Стекло")]
        Glass = 5,
        [Description("Батарейки")]
        Batteries = 6,
        [Description("Одежда")]
        Clothes = 7
    }
}