using System;
using Raylib;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using static Raylib.Raylib;

namespace ConsoleApp1
{
        class SceneObject
        {

            protected SceneObject parent = null;
            protected List<SceneObject> children = new List<SceneObject>();

            protected Matrix3 localTransform = new Matrix3();
            protected Matrix3 globalTransform = new Matrix3();

            public AABB box = new AABB(new Vector2(0,0), new Vector2(1,1));
            public Vector2 position = new Vector2(0,0);
            public List<Vector2> MyPoints = new List<Vector2>();
            Vector2 minVals = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            Vector2 maxVals = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

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
        public void Recalculating()
        {
             minVals = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
             maxVals = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

            for (int i = 0; i < MyPoints.Count; i++)
            {
                if (MyPoints[i].x + position.x < minVals.x)
                {
                    minVals.x = MyPoints[i].x + position.x;
                }
                if (MyPoints[i].y + position.y < minVals.y)
                {
                    minVals.y = MyPoints[i].y + position.y;
                }
                if (MyPoints[i].x + position.x > maxVals.x)
                {
                    maxVals.x = MyPoints[i].x + position.x;
                }
                if (MyPoints[i].y + position.y > maxVals.y)
                {
                    maxVals.y = MyPoints[i].y + position.y;
                }
            }
        }
        public void hitBox()
        {
            box.Fit2(MyPoints);
            box.min2 += position;
            box.max2 += position;
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
            Recalculating();
            hitBox();
            DrawLine((int)minVals.x, (int)minVals.y, (int)minVals.x, (int)maxVals.y, Color.GREEN);
            DrawLine((int)minVals.x, (int)maxVals.y, (int)maxVals.x, (int)maxVals.y, Color.GREEN);
            DrawLine((int)maxVals.x, (int)maxVals.y, (int)maxVals.x, (int)minVals.y, Color.GREEN);
            DrawLine((int)maxVals.x, (int)minVals.y, (int)minVals.x, (int)minVals.y, Color.GREEN);

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