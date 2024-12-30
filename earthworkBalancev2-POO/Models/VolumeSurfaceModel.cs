using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surface = Autodesk.Civil.DatabaseServices.Surface;

namespace earthworkBalancev2_POO.Models
{
    public class VolumeSurfaceModel
    {
        // Name of the surface
        public string VolumeSurfaceName { get; set; }
        public double UnadjustedNetVolume { get; set; }
        public double AdjustedNetVolume { get; set; }
        public double MeanVolumeElevation { get; set; }
        public double UnadjustedFillVolume { get; set; }
        public double UnadjustedCutVolume { get; set; }
        public double AdjustedFillVolume { get; set; }
        public double AdjustedCutVolume { get; set; }
        public ObjectId ObjectId { get; set; }
        private TinVolumeSurface _tinVolumeSurface { get; set; }
        private Surface _compairsonSurface { get; set; }
        private Surface _volumeSurface { get; set; }
        private TinSurface _tinSurface { get; set; }

        private void GetSurfaces(Surface volumeSurface)
        {
            TinSurface tinSurface = volumeSurface as TinSurface;
            if (tinSurface != null)
            {
                _tinSurface = tinSurface;
            }
            else
            {
                throw new Exception("Surface is not a TinSurface");
            }
            TinVolumeSurface tinVolumeSurface = volumeSurface as TinVolumeSurface;
            if (tinVolumeSurface != null)
            {
                _tinVolumeSurface = tinVolumeSurface;
            }
            else
            {
                throw new Exception("Surface is not a TinVolumeSurface");
            }
            Autodesk.AutoCAD.DatabaseServices.ObjectId compairsonSurface = _tinVolumeSurface.GetVolumeProperties().ComparisonSurface;
            if (_compairsonSurface != null)
            {
                _compairsonSurface = _compairsonSurface;
            }
            else
            {
                throw new Exception("Surface is not a TinSurface");
            }
        }
        private void GetVolumes(Surface volumeSurface)
        {
            UnadjustedNetVolume = _tinVolumeSurface.GetVolumeProperties().UnadjustedNetVolume;
            AdjustedNetVolume = _tinVolumeSurface.GetVolumeProperties().AdjustedNetVolume;
            MeanVolumeElevation = _tinSurface.GetGeneralProperties().MeanElevation;
            UnadjustedFillVolume = _tinVolumeSurface.GetVolumeProperties().UnadjustedFillVolume;
            UnadjustedCutVolume = _tinVolumeSurface.GetVolumeProperties().UnadjustedCutVolume;
            AdjustedFillVolume = _tinVolumeSurface.GetVolumeProperties().AdjustedFillVolume;
            AdjustedCutVolume = _tinVolumeSurface.GetVolumeProperties().AdjustedCutVolume;

        }

        public VolumeSurfaceModel(Surface volumeSurface)
        {
            _volumeSurface = volumeSurface ?? throw new ArgumentNullException(nameof(volumeSurface));
            VolumeSurfaceName = volumeSurface.Name;
            ObjectId = volumeSurface.ObjectId;
            GetSurfaces(volumeSurface);
            GetVolumes(volumeSurface);
        }
    }
}
