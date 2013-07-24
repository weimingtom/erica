using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Physics {
    /// <summary>
    /// レイ構造体
    /// </summary>
    /// <remarks>
    /// レイとは始点と終点がある1本の線分を伸ばしたものです。レイの長さはこの線分の長さの何倍かで表します。
    /// キャラの進行方向に障害物があるかどうかの判定などに使用されます。
    /// </remarks>
    public struct Ray {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="pointA">ポイントA（始点）</param>
        /// <param name="pointB">ポイントB（終点）</param>
        /// <param name="f">伸張量（A-Bの長さの倍数）</param>
        public Ray (Vector3 pointA, Vector3 pointB, float f)
            : this () {
            this.PointA = pointA;
            this.PointB = pointB;
            this.Fraction = f;
        }

        /// <summary>
        /// ポイントA（始点）
        /// </summary>
        public Vector3 PointA { get; private set; }

        /// <summary>
        /// ポイントB（終点）
        /// </summary>
        public Vector3 PointB { get; private set; }

        /// <summary>
        /// 伸張量
        /// </summary>
        /// <remarks>
        /// ポイントA-Bの長さの何倍かで表します。
        /// </remarks>
        public float Fraction { get; private set; }

        /// <summary>
        /// レイの始点
        /// </summary>
        /// <remarks>
        /// <see cref="PointA"/> と等価です。単なる別名です。
        /// </remarks>
        public Vector3 Origin { get { return PointA; } }

        /// <summary>
        /// レイの伸張方向
        /// </summary>
        /// <remarks>
        /// ポイントAからBに向かう単位長さのベクトルです。
        /// </remarks>
        public Vector3 Direction { get { return (PointB - PointA).Normalize (); } }

        /// <summary>
        /// レイの基本長さ
        /// </summary>
        /// <remarks>
        /// ポイントA-Bの距離です。
        /// </remarks>
        public float UnitLength { get { return (PointB - PointA).Length; } }

        /// <summary>
        /// レイの長さ
        /// </summary>
        /// <remarks>
        /// レイの基本超 <see cref="UnitLength"/> に伸張量 <see cref="Fraction"/> をかけたものです。
        /// </remarks>
        public float Length { get { return UnitLength * Fraction; } }
    }

    /// <summary>
    /// レイの公差判定の結果を受け取る構造体
    /// </summary>
    /// <remarks>
    /// レイの公差判定の結果を受け取ります。
    /// 基本的にはエンジン内部で使用する構造体です。
    /// ☆ LINQ構文でシンプルに書けるようにたくさんの情報を保持しています。
    /// </remarks>
    public struct RayIntersection {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="hit">ヒットしたかどうか</param>
        /// <param name="node">ヒットしたノード</param>
        /// <param name="normal">ヒットした地点の法線</param>
        /// <param name="f">ヒットした地点までの伸張量</param>
        /// <param name="ray">公差判定の元になったレイ</param>
        public RayIntersection (bool hit, Node node, Vector3 normal, float f, Ray ray)
            : this () {
            this.Hit = hit;
            this.Node = node;
            this.Normal = normal;
            this.Fraction = f;
            this.Ray = ray;
        }

        /// <summary>
        /// ヒット 
        /// </summary>
        public bool Hit { get; private set; }

        /// <summary>
        /// ヒットしたノード
        /// </summary>
        public Node Node { get; private set; }

        /// <summary>
        /// ヒットした地点の法線
        /// </summary>
        public Vector3 Normal { get; private set; }

        /// <summary>
        /// ヒットした地点までの伸張量
        /// </summary>
        public float Fraction { get; private set; }

        /// <summary>
        /// 公差判定の元になったレイ
        /// </summary>
        public Ray Ray { get; private set; }

        /// <summary>
        /// ヒットした地点までの長さ
        /// </summary>
        public float Distance { get { return Fraction * Ray.UnitLength; } }
    }



}
