using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Sphere
    {
        Vector3 center;
        float radius;

        public Sphere()
        {

        }

        public Sphere(Vector3 p, float r)
        {
            this.center = p;
            this.radius = r;
        }

        public void Fit(Vector3[] points)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int i = 0; i < points.Length; i++)
            {
                min = Vector3.Min(min, points[i]);
                max = Vector3.Max(max, points[i]);
            }

            center = (min + max) * .5f;
            radius = center.Distance(max);
        }

        public void Fit(List<Vector3> points)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }

            center = (min + max) * .5f;
            radius = center.Distance(max);
        }

        public bool Overlaps(Vector3 p)
        {
            Vector3 toPoint = p - center;
            return toPoint.MagnitudeSqr() <= (radius * radius);
        }

        public bool Overlaps(Sphere other)
        {
            Vector3 diff = other.center - center;

            float r = radius + other.radius;
            return diff.MagnitudeSqr() <= (r * r);
        }

        public bool Overlaps(AABB aabb)
        {
            Vector3 diff = aabb.ClosestPoint(center) - center;
            return diff.Dot(diff) <= (radius * radius);
        }

        Vector3 ClosestPoint(Vector3 p)
        {
            Vector3 toPoint = p - center;

            if (toPoint.MagnitudeSqr() > radius * radius)
            {
                toPoint = toPoint.GetNormalized() * radius;
            }
            return center + toPoint;
        }
    }
}
