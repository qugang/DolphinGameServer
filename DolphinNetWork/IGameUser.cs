using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinNetWork
{
    public interface IGameUser
    {
        string Sid { get; set; }
        string Uid { get; set; }
        string Pwd { get; set; }
        DateTime OnlimeDate { get; set; }
        GameUserState UserState { get; set; }
    }
}
