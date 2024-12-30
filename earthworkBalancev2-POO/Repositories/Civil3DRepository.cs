using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using earthworkBalancev2_POO.Models;
using System;
using System.Collections.Generic;
using Surface = Autodesk.Civil.DatabaseServices.Surface;

namespace earthworkBalancev2_POO.Repositories
{
    public class Civil3DRepository : ICivil3DRepository
    {
        private CivilDocument _civilDocument;
        private Database _database;
        private Transaction _transaction;
        private Editor _editor;

        private List<FeatureLine> _featureLines;
        private List<Surface> _surfaces;

        public Civil3DRepository(CivilDocument civilDocument, Database database, Transaction transaction, Editor editor)
        {
            _civilDocument = civilDocument ?? throw new ArgumentNullException(nameof(civilDocument));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));

            GetAllFeatureLines();
            GetAllSites();
        }

        public void EditFeatureLineElevation(string featureLineName, double elevation)
        {
            // Get feature line
            FeatureLine featureLine = GetFeatureLineByName(featureLineName);
            if (featureLine == null)
            {
                _editor.WriteMessage($"\nFeature line {featureLineName} not found.");
                return;
            }

            _editor.Command("RAISELOWERFEATURELINE", featureLine.ObjectId, elevation);
            // Check if the elevation changed
            FeatureLine updatedFeatureLine = UpdateFeatureLine(featureLineName);
            // Check for the initial elevation
            Point3d updateInitialElevation = updatedFeatureLine.GetPointAtParameter(0);
            Point3d existingInitialElevation = featureLine.GetPointAtParameter(0);
            if (updateInitialElevation.Z == existingInitialElevation.Z)
            {
                _editor.WriteMessage($"\nFeature line {featureLineName} elevation not changed.");
                // Raise an exception
                throw new Exception($"Feature line {featureLineName} elevation not changed.");
            }
            else
            {
                _editor.WriteMessage($"\nFeature line {featureLineName} elevation changed.");
            }
        }

        public List<FeatureLine> GetAllFeatureLines()
        {
            // Get sites
            List<Site> sites = GetAllSites();
            // Get feature lines from each site
            var featureLines = new List<FeatureLine>();
            foreach (Site site in sites)
            {
                ObjectIdCollection featureLineIDs = site.GetFeatureLineIds();
                foreach (ObjectId featureLineID in featureLineIDs)
                {
                    featureLines.Add(_transaction.GetObject(featureLineID, OpenMode.ForWrite) as FeatureLine);
                }
            }
            _featureLines = featureLines;
            return featureLines;
        }

        private List<Site> GetAllSites()
        {
            ObjectIdCollection siteIDs = _civilDocument.GetSiteIds();
            var sites = new List<Site>();
            foreach (ObjectId siteID in siteIDs)
            {
                sites.Add(_transaction.GetObject(siteID, OpenMode.ForRead) as Site);
            }
            return sites;
        }

        public List<Surface> GetAllSurfaces()
        {
            // Get all surfaces
            ObjectIdCollection surfaceIDs = _civilDocument.GetSurfaceIds();
            var surfaces = new List<Surface>();
            foreach (ObjectId surfaceID in surfaceIDs)
            {
                surfaces.Add(_transaction.GetObject(surfaceID, OpenMode.ForWrite) as Surface);
            }
            _surfaces = surfaces;
            return surfaces;
        }

        public FeatureLine GetFeatureLineByName(string featureLineName)
        {
            // Get sites
            List<Site> sites = GetAllSites();
            // Search for the feature line in each site
            foreach (Site site in sites)
            {
                ObjectIdCollection featureLineIDs = site.GetFeatureLineIds();
                foreach (ObjectId featureLineID in featureLineIDs)
                {
                    FeatureLine featureLine = _transaction.GetObject(featureLineID, OpenMode.ForWrite) as FeatureLine;
                    if (featureLine.Name == featureLineName)
                    {
                        return featureLine;
                    }
                }
            }
            return null;
        }

        public VolumeSurfaceModel GetSurfaceByName(string surfaceName)
        {
            // Search for the surface
            foreach (Surface surface in _surfaces)
            {
                if (surface.Name == surfaceName)
                {
                    var newSurfaceModel = new VolumeSurfaceModel(surface);
                    return newSurfaceModel;
                }
            }
            return null;
        }
        public FeatureLine UpdateFeatureLine(string featureLineName)
        {
            // Search for the feature line
            FeatureLine featureLine = GetFeatureLineByName(featureLineName);
            // Get from document the feature line
            FeatureLine updateFeatureLine = _transaction.GetObject(featureLine.ObjectId, OpenMode.ForWrite) as FeatureLine;
            return updateFeatureLine;

        }

        public Surface UpdateSurface(string surfaceName)
        {
            // Search for the surface
            VolumeSurfaceModel surface = GetSurfaceByName(surfaceName);
            // Get from document the surface
            Surface updateSurface = _transaction.GetObject(surface.ObjectId, OpenMode.ForWrite) as Surface;
            return updateSurface;
        }
    }
}
