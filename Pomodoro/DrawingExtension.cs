using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pomodoro
{
    /// <summary>
    /// 그리기 확장
    /// </summary>
    public static class DrawingExtension
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region 호 추가하기 - AddArc(canvas, fillBrush, strokeBrush, strokeThickness, startPoint, endPoint, size, rotationAngle, isLargeArc, sweepDirection, isStroked)

        /// <summary>
        /// 호 추가하기
        /// </summary>
        /// <param name="canvas">캔버스</param>
        /// <param name="fillBrush">칠하기 브러시</param>
        /// <param name="strokeBrush">스트로크 브러시</param>
        /// <param name="strokeThickness">스트로크 두께</param>
        /// <param name="startPoint">시작 포인트</param>
        /// <param name="endPoint">종료 포인트</param>
        /// <param name="size">크기</param>
        /// <param name="rotationAngle">회전 각도</param>
        /// <param name="isLargeArc">큰 아크 여부</param>
        /// <param name="sweepDirection">스윕 방향</param>
        /// <param name="isStroked">스트로크 여부</param>
        /// <returns>호</returns>
        public static Path AddArc
        (
            this Canvas    canvas,
            Brush          fillBrush,
            Brush          strokeBrush,
            double         strokeThickness,
            Point          startPoint,
            Point          endPoint,
            Size           size,
            double         rotationAngle,
            bool           isLargeArc,
            SweepDirection sweepDirection,
            bool           isStroked
        )
        {
            #region 패스 지오메트리를 설정한다.

            PathGeometry pathGeometry = new PathGeometry();

            #endregion
            #region 패스 세그먼트 컬렉션을 설정한다.

            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();

            #endregion
            #region 호 세그먼트를 설정한다.

            ArcSegment arcSegment = new ArcSegment
            (
                endPoint,
                size,
                rotationAngle,
                isLargeArc,
                sweepDirection,
                isStroked
            );

            pathSegmentCollection.Add(arcSegment);

            #endregion
            #region 패스 모양을 설정한다.

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = startPoint;
            pathFigure.Segments   = pathSegmentCollection;

            pathGeometry.Figures.Add(pathFigure);

            #endregion
            #region 패스를 설정한다.

            Path path = new Path();

            path.Stroke          = strokeBrush;
            path.StrokeThickness = strokeThickness;
            path.Fill            = fillBrush;
            path.Data            = pathGeometry;

            canvas.Children.Add(path);

            #endregion

            return path;
        }

        #endregion
        #region 호 추가하기 - AddArc(canvas, fillBrush, strokeBrush, strokeThickness, rect, angle1, angle2, isLargeArc, sweepDirection, point1, point2)

        /// <summary>
        /// 호 추가하기
        /// </summary>
        /// <param name="canvas">캔버스</param>
        /// <param name="fillBrush">칠하기 브러시</param>
        /// <param name="strokeBrush">스트로크 브러시</param>
        /// <param name="strokeThickness">스트로크 두께</param>
        /// <param name="rect">사각형</param>
        /// <param name="angle1">각도 1</param>
        /// <param name="angle2">각도 2</param>
        /// <param name="isLargeArc">큰 아크 여부</param>
        /// <param name="sweepDirection">스윕 방향</param>
        /// <param name="point1">포인트 1</param>
        /// <param name="point2">포인트 2</param>
        /// <returns>호</returns>
        public static Path AddArc
        (
            this Canvas    canvas,
            Brush          fillBrush,
            Brush          strokeBrush,
            double         strokeThickness,
            Rect           rect,
            double         angle1,
            double         angle2,
            bool           isLargeArc,
            SweepDirection sweepDirection,
            out Point      point1,
            out Point      point2
        )
        {
            Point[] pointArray = FindEllipsePointArray(rect, angle1, angle2);

            point1 = pointArray[0];
            point2 = pointArray[1];

            Size size = new Size(rect.Width / 2, rect.Height / 2);

            return canvas.AddArc
            (
                fillBrush,
                strokeBrush,
                strokeThickness,
                pointArray[0],
                pointArray[1],
                size,
                0,
                isLargeArc,
                sweepDirection,
                true
            );
        }

        #endregion

        #region 파이 슬라이스 추가하기 - AddPieSlice(canvas, fillBrush, strokeBrush, strokeThickness, rect, angle1, angle2, isLargeArc, sweepFirection, point1, point2)

        /// <summary>
        /// 파이 슬라이스 추가하기
        /// </summary>
        /// <param name="canvas">캔버스</param>
        /// <param name="fillBrush">칠하기 브러시</param>
        /// <param name="strokeBrush">스트로크 브러시</param>
        /// <param name="strokeThickness">스트로크 두께</param>
        /// <param name="rect">사각형</param>
        /// <param name="angle1">각도 1</param>
        /// <param name="angle2">각도 2</param>
        /// <param name="isLargeArc">큰 아크 여부</param>
        /// <param name="sweepFirection">스윕 방향</param>
        /// <param name="point1">포인트 1</param>
        /// <param name="point2">포인트 2</param>
        /// <returns>파이 슬라이스 패스</returns>
        public static Path AddPieSlice
        (
            this Canvas    canvas,
            Brush          fillBrush,
            Brush          strokeBrush,
            double         strokeThickness,
            Rect           rect,
            double         angle1,
            double         angle2,
            bool           isLargeArc,
            SweepDirection sweepFirection,
            out Point      point1,
            out Point      point2
        )
        {
            Path path = canvas.AddArc
            (
                fillBrush,
                strokeBrush,
                strokeThickness,
                rect,
                angle1,
                angle2,
                isLargeArc,
                sweepFirection,
                out point1,
                out point2
            );

            PathGeometry pathGeometry = (PathGeometry)path.Data;

            PathFigureCollection pathFigureCollection = pathGeometry.Figures;

            PathFigure pathFigure = pathFigureCollection[0];

            PathSegmentCollection pathSegmentCollection = pathFigure.Segments;

            Point centerPoint = new Point
            (
                (rect.Left + rect.Right ) / 2,
                (rect.Top  + rect.Bottom) / 2
            );

            LineSegment lineSegment1 = new LineSegment(centerPoint, true);

            pathSegmentCollection.Add(lineSegment1);
            
            LineSegment lineSegment2 = new LineSegment(point1, true);

            pathSegmentCollection.Add(lineSegment2);

            return path;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////// Private

        #region 타원 세그먼트 교차점 배열 찾기 - FindEllipseSegmentIntersectionPointArray(rect, point1, point2, segmentOnly)

        /// <summary>
        /// 타원 세그먼트 교차점 배열 찾기
        /// </summary>
        /// <param name="rect">사각형</param>
        /// <param name="point1">포인트 1</param>
        /// <param name="point2">포인트 2</param>
        /// <param name="segmentOnly">세그먼트 전용 여부</param>
        /// <returns>타원 세그먼트 교차점 배열</returns>
        private static Point[] FindEllipseSegmentIntersectionPointArray(Rect rect, Point point1, Point point2, bool segmentOnly)
        {
            if((rect.Width == 0) || (rect.Height == 0) || ((point1.X == point2.X) && (point1.Y == point2.Y)))
            {
                return new Point[] { };
            }

            if(rect.Width < 0)
            {
                rect.X     =  rect.Right;
                rect.Width = -rect.Width;
            }

            if(rect.Height < 0)
            {
                rect.Y      =  rect.Bottom;
                rect.Height = -rect.Height;
            }

            double centerX = rect.Left + rect.Width  / 2f;
            double centerY = rect.Top  + rect.Height / 2f;

            rect.X -= centerX;
            rect.Y -= centerY;

            point1.X -= centerX;
            point1.Y -= centerY;
            point2.X -= centerX;
            point2.Y -= centerY;

            double a = rect.Width  / 2;
            double b = rect.Height / 2;

            double A = (point2.X - point1.X) * (point2.X - point1.X) / a / a + (point2.Y - point1.Y) * (point2.Y - point1.Y) / b / b;
            double B = 2 * point1.X * (point2.X - point1.X) / a / a + 2 * point1.Y * (point2.Y - point1.Y) / b / b;
            double C = point1.X * point1.X / a / a + point1.Y * point1.Y / b / b - 1;

            List<double> valueList = new List<double>();

            double discriminant = B * B - 4 * A * C;

            if(discriminant == 0)
            {
                valueList.Add(-B / 2 / A);
            }
            else if(discriminant > 0)
            {
                valueList.Add((double)((-B + Math.Sqrt(discriminant)) / 2 / A));
                valueList.Add((double)((-B - Math.Sqrt(discriminant)) / 2 / A));
            }

            List<Point> pointList = new List<Point>();

            foreach(double value in valueList)
            {
                if(!segmentOnly || ((value >= 0f) && (value <= 1f)))
                {
                    double x = point1.X + (point2.X - point1.X) * value + centerX;
                    double y = point1.Y + (point2.Y - point1.Y) * value + centerY;

                    pointList.Add(new Point(x, y));
                }
            }

            return pointList.ToArray();
        }

        #endregion
        #region 타원 포인트 배열 찾기 - FindEllipsePointArray(rect, angle1, angle2)

        /// <summary>
        /// 타원 포인트 배열 찾기
        /// </summary>
        /// <param name="rect">사각형</param>
        /// <param name="angle1">각도 1</param>
        /// <param name="angle2">각도 2</param>
        /// <returns>타원 포인트 배열</returns>
        private static Point[] FindEllipsePointArray(Rect rect, double angle1, double angle2)
        {
            Point centerPoint = new Point
            (
                rect.X + rect.Width  / 2.0,
                rect.Y + rect.Height / 2.0
            );

            double distance = rect.Width + rect.Height;

            Point point1 = new Point
            (
                centerPoint.X + distance * Math.Cos(angle1),
                centerPoint.Y + distance * Math.Sin(angle1)
            );

            Point point2 = new Point
            (
                centerPoint.X + distance * Math.Cos(angle2),
                centerPoint.Y + distance * Math.Sin(angle2)
            );

            Point[] intersectionPointArray1 = FindEllipseSegmentIntersectionPointArray(rect, centerPoint, point1, true);
            Point[] intersectionPointArray2 = FindEllipseSegmentIntersectionPointArray(rect, centerPoint, point2, true);

            return new Point[]
            {
                intersectionPointArray1[0],
                intersectionPointArray2[0]
            };
        }

        #endregion
    }
}