namespace SimpleTGBot;

public class SongTab
{
    private String name;
    private String pathToTab;


    public SongTab(string name, string pathToTab)
    {
        this.name = name;
        this.pathToTab = pathToTab;
    }

    public static SongTab[] getMyFavouriteTabs()
    {
        String[] path = {"березы любэ.txt","девочка пай.txt","кто такая элиз.txt","лирическая.txt",
            "самый лучший ден.txt","седьмой лепесток.txt","супермаркет.txt","этот город браво кап 3л.txt" };
        SongTab[] res = new SongTab[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            res[i] = new SongTab(path[i].Split(".")[0], path[i]);
        }

        return res;
    }
    
    public static SongTab[] getNervesTabs()
    {
        String[] path = {"березы любэ.txt","девочка пай.txt","кто такая элиз.txt","лирическая.txt",
            "самый лучший ден.txt","седьмой лепесток.txt","супермаркет.txt","этот город браво кап 3л.txt" };
        SongTab[] res = new SongTab[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            res[i] = new SongTab(path[i].Split(".")[0], path[i]);
        }

        return res;
    }
    
    public static SongTab[] getZveriTabs()
    {
        String[] path = {"березы любэ.txt","девочка пай.txt","кто такая элиз.txt","лирическая.txt",
            "самый лучший ден.txt","седьмой лепесток.txt","супермаркет.txt","этот город браво кап 3л.txt" };
        SongTab[] res = new SongTab[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            res[i] = new SongTab(path[i].Split(".")[0], path[i]);
        }

        return res;
    }
    public static SongTab[] getSplinTabs()
    {
        String[] path = {"березы любэ.txt","девочка пай.txt","кто такая элиз.txt","лирическая.txt",
            "самый лучший ден.txt","седьмой лепесток.txt","супермаркет.txt","этот город браво кап 3л.txt" };
        SongTab[] res = new SongTab[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            res[i] = new SongTab(path[i].Split(".")[0], path[i]);
        }

        return res;
    }

    public override string ToString()
    {
        return pathToTab;
    }
}