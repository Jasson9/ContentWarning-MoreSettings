using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSettings.Settings.Type
{
    public interface IPatch
    {
        void ApplyPatch(ref Harmony harmony);
    }
}
