﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 俺数学ライブラリ
    /// </summary>
    public static class MyMath {
        
        /// <summary>
        /// PI
        /// </summary>
        public static float PI {
            get { return (float)Math.PI; }
        }

        /// <summary>
        /// [最小値,最大値]へのクランプ
        /// </summary>
        /// <param name="value">クランプする値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public static float Clamp (float value, float min, float max) {
            return Math.Max (min, Math.Min (value, max));
        }
    }
}
