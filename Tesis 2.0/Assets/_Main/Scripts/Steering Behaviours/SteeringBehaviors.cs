using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    public static class SteeringBehaviors
    {
        /// <returns>
        ///   <para>Devuelve una direccion directa para INTERCEPTAR  al objetivo X SEGUNDOS adelante en su trayectoria.</para>
        /// </returns>
        public static Vector2 GetInterceptDir(Vector2 p_origin, Vector2 p_targetPos, Vector2 p_targetLookDir,float p_targetSpeed, float p_time)
        {
            Vector2 l_point = p_targetPos + (p_targetLookDir * Mathf.Clamp(p_targetSpeed * p_time, 0, 100));
            return (l_point - p_origin).normalized;
        }
        /// <returns>
        ///   <para>Devuelve una direccion directa para HUIR  del la futura posicion del objetivo X SEGUNDOS adelante.</para>
        /// </returns>
        public static Vector2 GetEvadeDir(Vector2 p_originPos, Vector2 p_targetPos, Vector2 p_targetLookDir,float p_targetSpeed, float p_time)
        {
            Vector2 l_point = p_targetPos + (p_targetLookDir * Mathf.Clamp(p_targetSpeed * p_time, 0, 100));
            return -(l_point - p_originPos).normalized;
        }
        /// <returns>
        ///   <para>Devuelve una direccion directa para ir HUIR el objetivo.</para>
        /// </returns>
        public static Vector2 GetFleeDir(Vector2 p_originPos, Vector2 p_targetPos)
        {
            return -(p_targetPos -p_originPos).normalized;
        }
        
        /// <returns>
        ///   <para>Devuelve una direccion directa para IR HACIA el objetivo.</para>
        /// </returns>
        public static Vector3 GetChaseDir(Vector2 p_originPos, Vector2 p_targetPos)
        {
            return (p_targetPos - p_originPos).normalized;
        }
        
        
        /// <summary>
        ///   <para> Obstacle Avoidace</para>
        /// </summary>
        /// <param name="p_detectionRaduis"> El radio de deteccion para obstaculos.</param>
        /// <param name="p_viewAngle"> Es el angulo de vision al que la entidad presta atencion para ver obstaculos.</param>
        /// <returns>
        ///   <para>Devuelve una direccion optima para esquivar todos los obstaculos dentro del rango.</para>
        /// </returns>
        public static Vector3 GetObsAvoidanceDir(Vector2 p_orginPos, Vector2 p_originLookDir ,float p_detectionRaduis, float p_viewAngle,LayerMask l_mask)
        {
            
            var l_allObs = new Collider[10];
            var l_countObs = Physics.OverlapSphereNonAlloc(p_orginPos, p_detectionRaduis, l_allObs, l_mask);
            Vector2 l_dirToAvoid = Vector3.zero;
            int l_trueObs = 0;
            for (int l_i = 0; l_i < l_countObs; l_i++)
            {
                var l_currObs = l_allObs[l_i];
                var l_closestPoint = l_currObs.ClosestPointOnBounds(p_orginPos);
                var l_diffToPoint = (Vector2)l_closestPoint - p_orginPos;
                
                var l_angleToPoint = Vector3.Angle(p_originLookDir, l_diffToPoint.normalized);
                
                if(l_angleToPoint > p_viewAngle/2) continue;
                float l_dist = l_diffToPoint.magnitude;
                
                l_trueObs++;
                l_dirToAvoid += -(l_diffToPoint).normalized * (p_detectionRaduis - l_dist);

            }
            
            if(l_trueObs != 0)
                l_dirToAvoid /= l_trueObs;

            return l_dirToAvoid;
        }
        
    }
}