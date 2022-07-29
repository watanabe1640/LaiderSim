using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HUBONE.Visualize
{
    public class TestDrawMesh : MonoBehaviour
    {

        [Header("DrawMeshInstancedIndirectのパラメータ")]
        [SerializeField]
        private Mesh m_mesh;

        [SerializeField]
        private Material m_instanceMaterial;

        [SerializeField]
        private Bounds m_bounds;

        public  ComputeBuffer m_argsBuffer;

        private ComputeBuffer m_positionBuffer;

        private ComputeBuffer m_eulerAngleBuffer;

        private int m_instanceCount;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void DrawmeshIndirect(List<Vector3> positions)
        {
            if (positions.Count == 0 || positions == null)
            {
                return;
            }
            this.m_instanceCount = positions.Count;

            SetArgsBuffer();
            SetPositionBuffer(positions);
            SetEulerAngleBuffer();

            Graphics.DrawMeshInstancedIndirect(
                m_mesh,
                0,
                m_instanceMaterial,
                m_bounds,
                m_argsBuffer,
                0,
                null,
                ShadowCastingMode.Off,
                false
            );
        }

        public void SetArgsBuffer()
        {

            uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

            uint numIndices = (m_mesh != null) ? (uint)m_mesh.GetIndexCount(0) : 0;

            args[0] = numIndices;
            args[1] = (uint)m_instanceCount;

            m_argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            m_argsBuffer.SetData(args);

        }

        public void SetPositionBuffer(List<Vector3> in_positions)
        {

            // xyz:座標   w:スケール
            Vector4[] positions = new Vector4[m_instanceCount];

            for (int i = 0; i < m_instanceCount; ++i)
            {

                positions[i].x = in_positions[i].x;
                positions[i].y = in_positions[i].y;
                positions[i].z = in_positions[i].z;

                positions[i].w = 0.03f;

            }

            m_positionBuffer = new ComputeBuffer(m_instanceCount, 4 * 4);
            m_positionBuffer.SetData(positions);

            m_instanceMaterial.SetBuffer("positionBuffer", m_positionBuffer);

        }

        public void SetEulerAngleBuffer()
        {

            Vector3[] angles = new Vector3[m_instanceCount];

            for (int i = 0; i < m_instanceCount; ++i)
            {
                angles[i].x = 0;
                angles[i].y = 0;
                angles[i].z = 0;
            }

            m_eulerAngleBuffer = new ComputeBuffer(m_instanceCount, 4 * 3);
            m_eulerAngleBuffer.SetData(angles);

            m_instanceMaterial.SetBuffer("eulerAngleBuffer", m_eulerAngleBuffer);

        }
        public void Disable()
        {
            if (m_positionBuffer != null)
                m_positionBuffer.Release();
            m_positionBuffer = null;

            if (m_eulerAngleBuffer != null)
                m_eulerAngleBuffer.Release();
            m_eulerAngleBuffer = null;

            if (m_argsBuffer != null)
                m_argsBuffer.Release();
            m_argsBuffer = null;

        }
        // 領域の解放
        private void OnDisable()
        {

            if (m_positionBuffer != null)
                m_positionBuffer.Release();
            m_positionBuffer = null;

            if (m_eulerAngleBuffer != null)
                m_eulerAngleBuffer.Release();
            m_eulerAngleBuffer = null;

            if (m_argsBuffer != null)
                m_argsBuffer.Release();
            m_argsBuffer = null;

        }

    }
}
