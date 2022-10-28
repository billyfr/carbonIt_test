using System.Collections.Generic;
public interface IGameManagerErrors
{
    void CheckErrorEntry(string[] arg);
    void CheckAdventurerEntries(List<string> options);
    void CheckTreasureEntries(List<string> options);
    void CheckMapOrMountainEntries(List<string> options);

}