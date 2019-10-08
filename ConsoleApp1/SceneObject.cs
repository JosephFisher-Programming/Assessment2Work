using System;
using Raylib;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Matrix3
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9;
        public static Matrix3 identity = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        Matrix3 GetTransposed()
        {
            return new Matrix3(m1, m4, m7, m2, m5, m8, m3, m6, m9);
        }
        public Matrix3(float val1, float val2, float val3, float val4, float val5, float val6, float val7, float val8, float val9)
        {
            m1 = val1; m2 = val2; m3 = val3;
            m4 = val4; m5 = val5; m6 = val6;
            m7 = val7; m8 = val8; m9 = val9;
        }

        public Matrix3()
        {
            m1 = 1; m2 = 0; m3 = 0;
            m4 = 0; m5 = 1; m6 = 0;
            m7 = 0; m8 = 0; m9 = 1;
        }

        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m1 * rhs.m1 + lhs.m4 * rhs.m2 + lhs.m7 * rhs.m3,
                lhs.m2 * rhs.m1 + lhs.m5 * rhs.m2 + lhs.m8 * rhs.m3,
                lhs.m3 * rhs.m1 + lhs.m6 * rhs.m2 + lhs.m9 * rhs.m3,

                lhs.m1 * rhs.m4 + lhs.m4 * rhs.m5 + lhs.m7 * rhs.m6,
                lhs.m2 * rhs.m4 + lhs.m5 * rhs.m5 + lhs.m8 * rhs.m6,
                lhs.m3 * rhs.m4 + lhs.m6 * rhs.m5 + lhs.m9 * rhs.m6,

                lhs.m1 * rhs.m7 + lhs.m4 * rhs.m8 + lhs.m7 * rhs.m9,
                lhs.m2 * rhs.m7 + lhs.m5 * rhs.m8 + lhs.m8 * rhs.m9,
                lhs.m3 * rhs.m7 + lhs.m6 * rhs.m8 + lhs.m9 * rhs.m9);
        }
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m2 = 0; m3 = 0;
            m4 = 0; m5 = y; m6 = 0;
            m7 = 0; m8 = 0; m9 = z;
        }
        void Set(Matrix3 m)
        {
            m1 = m.m1; m2 = m.m2; m3 = m.m3;
            m4 = m.m4; m5 = m.m5; m6 = m.m6;
            m7 = m.m7; m8 = m.m8; m9 = m.m9;
        }
        void Set(float val1, float val2, float val3, float val4, float val5, float val6, float val7, float val8, float val9)
        {
            m1 = val1; m2 = val2; m3 = val3;
            m4 = val4; m5 = val5; m6 = val6;
            m7 = val7; m8 = val8; m9 = val9;
        }
        public void SetRotateX(double radians)
        {
            Set(1, 0, 0, 0, (float)Math.Cos(radians), (float)Math.Sin(radians), 0, (float)-Math.Sin(radians), (float)Math.Cos(radians));
        }
        public void SetRotateY(double radians)
        {
            Set((float)Math.Cos(radians), 0, (float)Math.Sin(radians), 0, 1, 0, (float)-Math.Sin(radians), 0, (float)Math.Cos(radians));
        }
        public void SetRotateZ(double radians)
        {
            Set((float)Math.Cos(radians), (float)-Math.Sin(radians), 0, (float)Math.Sin(radians), (float)Math.Cos(radians), 0, 0, 0, 1);
        }
        public void RotateX(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateX(radians);

            Set(this * m);
        }
        public void RotateY(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateY(radians);

            Set(this * m);
        }
        public void RotateZ(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateZ(radians);

            Set(this * m);
        }
        public void Scale(float x, float y, float z)
        {
            Matrix3 m = new Matrix3();
            m.SetScaled(x, y, z);
            Set(this * m);
        }
        void SetEuler(float pitch, float yaw, float roll)
        {
            Matrix3 x = new Matrix3();
            Matrix3 y = new Matrix3();
            Matrix3 z = new Matrix3();
            x.SetRotateX(pitch);
            y.SetRotateX(yaw);
            z.SetRotateX(roll);

            Set(z * y * x);
        }
        public void SetTranslation(float x, float y)
        {
            m7 = x; m8 = y; m9 = 1;
        }
        public void Translate(float x, float y)
        {
            m7 += x; m8 += y;
        }
    }
        class SceneObject
        {
            protected SceneObject parent = null;
            protected List<SceneObject> children = new List<SceneObject>();

            protected Matrix3 localTransform = new Matrix3();
            protected Matrix3 globalTransform = new Matrix3();

            public Matrix3 LocalTransform
            {
                get { return localTransform; }
            }

            public Matrix3 GlobalTransform
            {
                get { return GlobalTransform; }
            }

            public SceneObject Parent
            {
                get { return parent; }
            }

            public SceneObject()
            {

            }

            public int GetChildCount()
            {
                return children.Count;
            }

            public SceneObject GetChild(int index)
            {
                return children[index];
            }

            public void AddChild(SceneObject child)
            {
                Debug.Assert(child.parent == null);
                child.parent = this;
                children.Add(child);
            }

            public void RemoveChild(SceneObject child)
            {
                if (children.Remove(child) == true)
                {
                    child.parent = null;
                }
            }
            ~SceneObject()
            {
                if (parent != null)
                {
                    parent.RemoveChild(this);
                }
                foreach (SceneObject so in children)
                {
                    so.parent = null;
                }
            }

            public virtual void OnUpdate(float deltaTime)
            {

            }

            public virtual void OnDraw()
            {

            }

            public void Update(float deltaTime)
            {
                OnUpdate(deltaTime);

                foreach (SceneObject child in children)
                {
                    child.Update(deltaTime);
                }
            }

            public void Draw()
            {
                OnDraw();

                foreach (SceneObject child in children)
                {
                    child.Draw();
                }
            }
            public void SetPosition(float x, float y)
            {
                localTransform.SetTranslation(x, y);
                UpdateTransform();
            }
            public void SetRotate(float radians)
            {
                localTransform.SetRotateZ(radians);
                UpdateTransform();
            }
            public void SetScale(float width, float height)
            {
                localTransform.SetScaled(width, height, 1);
                UpdateTransform();
            }
            public void Translate(float x, float y)
            {
                localTransform.Translate(x, y);
                UpdateTransform();
            }
            public void Rotate(float radians)
            {
                localTransform.RotateZ(radians);
                UpdateTransform();
            }
            public void Scale(float width, float height)
            {
                localTransform.Scale(width, height, 1);
                UpdateTransform();
            }
            void UpdateTransform()
            {
                if (parent != null)
                    globalTransform = parent.globalTransform * localTransform;
                else
                    globalTransform = localTransform;

                foreach (SceneObject child in children)
                    child.UpdateTransform();
            }
        }
    }