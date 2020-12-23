﻿using Eldemarkki.VoxelTerrain.World.Chunks;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Eldemarkki.VoxelTerrain.Chunks
{
    /// <summary>
    /// Provider for procedurally generated chunks
    /// </summary>
    public class ProceduralChunkProvider : ChunkProvider
    {
        /// <summary>
        /// The maximum amount of chunks that can be generated in one frame
        /// </summary>
        [SerializeField] private int chunkGenerationRate = 10;

        /// <summary>
        /// A list that contains all the coordinates where a chunk will eventually have to be generated
        /// </summary>
        private List<int3> _generationQueue;

        private void Awake()
        {
            _generationQueue = new List<int3>();
        }

        private void Update()
        {
            int chunksGenerated = 0;
            while (_generationQueue.Count > 0 && chunksGenerated < chunkGenerationRate)
            {
                int3 chunkCoordinate = _generationQueue[0];
                _generationQueue.RemoveAt(0);

                if (VoxelWorld.ChunkStore.TryGetChunkAtCoordinate(chunkCoordinate, out ChunkProperties chunkProperties))
                {               
                    if (!chunkProperties.IsMeshGenerated)
                    {
                        VoxelWorld.ChunkUpdater.GenerateVoxelDataAndMeshImmediate(chunkProperties);
                        chunksGenerated++;
                    }
                }
            }
        }

        /// <summary>
        /// Ensures that a chunk exists at a coordinate, if there is not, a new chunk is instantiated there, and its will eventually be generated
        /// </summary>
        /// <param name="chunkCoordinate">The chunk's coordinate</param>
        public void EnsureChunkExistsAtCoordinate(int3 chunkCoordinate)
        {
            if (!VoxelWorld.ChunkStore.DoesChunkExistAtCoordinate(chunkCoordinate))
            {
                CreateUnloadedChunkToCoordinate(chunkCoordinate);
                AddChunkToGenerationQueue(chunkCoordinate);
            }
        }

        public void AddChunkToGenerationQueue(int3 chunkCoordinate)
        {
            _generationQueue.Add(chunkCoordinate);
        }
    }
}