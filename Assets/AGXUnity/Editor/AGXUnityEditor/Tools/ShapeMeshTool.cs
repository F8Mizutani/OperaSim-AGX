﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using AGXUnity;
using GUI = AGXUnity.Utils.GUI;

namespace AGXUnityEditor.Tools
{
  [CustomTool( typeof( AGXUnity.Collide.Mesh ) )]
  public class ShapeMeshTool : ShapeTool
  {
    public AGXUnity.Collide.Mesh Mesh { get { return Shape as AGXUnity.Collide.Mesh; } }

    public ShapeMeshTool( Object[] targets )
      : base( targets )
    {
    }

    public override void OnAdd()
    {
      base.OnAdd();
    }

    public override void OnRemove()
    {
      base.OnRemove();
    }

    public override void OnPreTargetMembersGUI()
    {
      base.OnPreTargetMembersGUI();

      var sourceObjects = Mesh.SourceObjects;
      var singleSource  = sourceObjects.FirstOrDefault();

      if ( IsMultiSelect ) {
        var undoCollection = new List<Object>();
        foreach ( var target in GetTargets<AGXUnity.Collide.Mesh>() )
          if ( target != null )
            undoCollection.AddRange( target.GetUndoCollection() );
        Undo.RecordObjects( undoCollection.ToArray(), "Mesh source" );
      }
      else
        Undo.RecordObjects( Mesh.GetUndoCollection(), "Mesh source" );

      ShapeMeshSourceGUI( singleSource, newSource =>
      {
        if ( IsMultiSelect ) {
          foreach ( var target in GetTargets<AGXUnity.Collide.Mesh>() )
            if ( target != null )
              target.SetSourceObject( newSource );
        }
        else
          Mesh.SetSourceObject( newSource );
      } );
    }

    public static void ShapeMeshSourceGUI( Mesh currentSource,
                                           System.Action<Mesh> onNewMesh )
    {
      var newSource = EditorGUILayout.ObjectField( GUI.MakeLabel( "Source" ),
                                                   currentSource,
                                                   typeof( Mesh ),
                                                   false ) as Mesh;
      if ( newSource != currentSource )
        onNewMesh?.Invoke( newSource );
    }
  }
}
