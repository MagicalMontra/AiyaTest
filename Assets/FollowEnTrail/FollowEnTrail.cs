/*
    The following license supersedes all notices in the source code.
*/

/*
	Copyright (c) 2017 Kurt Dekker/PLBM Games All rights reserved.

	http://www.twitter.com/kurtdekker

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are
	met:

	Redistributions of source code must retain the above copyright notice,
	this list of conditions and the following disclaimer.

	Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	Neither the name of the Kurt Dekker/PLBM Games nor the names of its
	contributors may be used to endorse or promote products derived from
	this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
	IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
	TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
	PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
	TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
	PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
	LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowEnTrail : MonoBehaviour
{
	public enum FollowType { TIME, DISTANCE, };

	public class FollowClient
	{
		public Transform client;
		public FollowType followType;
		public float entrail;
	}

	public class PositionInTime
	{
		public PositionInTime( Vector3 _position, float _time)
		{
			position = _position;
			time = _time;
		}
		public Vector3 position;
		public float time;

		public float deltaTime;
		public float deltaDistance;
	}
	
	Transform leader;
	List<PositionInTime> positionHistory = new List<PositionInTime> ();

	List<FollowClient> clients = new List<FollowClient> ();

	public static FollowEnTrail Create( Transform leader)
	{
		FollowEnTrail fet = new GameObject (
			System.String.Format ("FollowEnTrail.Create({0});", leader.name)).
			AddComponent<FollowEnTrail> ();
		fet.leader = leader;
		return fet;
	}

	public void AddDistanceClient( Transform client, float distance)
	{
		FollowClient fc = new FollowClient ();
		fc.client = client;
		fc.followType = FollowType.DISTANCE;
		fc.entrail = distance;
		fc.client.position = leader.position;
		clients.Add (fc);
	}

	public void AddTimeClient( Transform client, float time)
	{
		FollowClient fc = new FollowClient ();
		fc.client = client;
		fc.followType = FollowType.TIME;
		fc.entrail = time;
		fc.client.position = leader.position;
		clients.Add (fc);
	}
	
	void LateUpdate()
	{
		float now = Time.time;

		PositionInTime newPit = new PositionInTime (leader.position, now);

		if (positionHistory.Count > 0)
		{
			PositionInTime pit = positionHistory [positionHistory.Count - 1];
			newPit.deltaDistance = Vector3.Distance( newPit.position, pit.position);
			newPit.deltaTime = newPit.time - pit.time;
		}

		positionHistory.Add ( newPit);

		foreach( FollowClient fc in clients)
		{
			switch( fc.followType)
			{
			case FollowType.TIME :
			{
				float remainingEntrailTime = fc.entrail;
				for (int i = positionHistory.Count - 1; i > 0; i--)
				{
					if (remainingEntrailTime <= positionHistory[i].deltaTime)
					{
						float a = 0;
						if (positionHistory[i].deltaTime > 0)
						{
							a = remainingEntrailTime / positionHistory[i].deltaTime;
						}
						fc.client.position = Vector3.Lerp(
							positionHistory[i].position,
							positionHistory[i-1].position,
							a);
						break;
					}
					remainingEntrailTime -= positionHistory[i].deltaTime;
				}
			}
				break;
			case FollowType.DISTANCE :
			{
				float remainingEntrailDistance = fc.entrail;
				for (int i = positionHistory.Count - 1; i > 0; i--)
				{
					if (remainingEntrailDistance <= positionHistory[i].deltaDistance)
					{
						float a = 0;
						if (positionHistory[i].deltaDistance > 0)
						{
							a = remainingEntrailDistance / positionHistory[i].deltaDistance;
						}
						fc.client.position = Vector3.Lerp(
							positionHistory[i].position,
							positionHistory[i-1].position,
							a);
						break;
					}
					remainingEntrailDistance -= positionHistory[i].deltaDistance;
				}
			}
				break;
			}
		}
	}
}
