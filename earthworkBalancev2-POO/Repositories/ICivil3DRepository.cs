using Autodesk.Civil.DatabaseServices;
using earthworkBalancev2_POO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace earthworkBalancev2_POO.Repositories
{
    public interface ICivil3DRepository
    {
        List<FeatureLine> GetAllFeatureLines();
        List<Surface> GetAllSurfaces();
        VolumeSurfaceModel GetSurfaceByName(string surfaceName);
        FeatureLine GetFeatureLineByName(string featureLineName);
        Surface UpdateSurface(string surfaceName);
        void EditFeatureLineElevation(string featureLineName, double elevation);
        FeatureLine UpdateFeatureLine(string featureLineName);

    }
}
