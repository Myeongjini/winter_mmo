using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();
        
        object _lock = new object();
        Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        int _roomId = 1;

        // 타입 왜 이렇게 되는지 생각 내가 해보기 ... (익숙하셔서 그런가)
        public GameRoom Add(int mapId)
        {
            GameRoom gameRoom = new GameRoom();
            gameRoom.Init(mapId);

            lock (_lock)
            {
                gameRoom.RoomId = _roomId;
                _rooms.Add(_roomId, gameRoom);
                _roomId++;
            }

            return gameRoom;
        }

        public bool Remove(int roomId)
        {
            lock (_lock)
            {
                return _rooms.Remove(roomId);
            }
        }

        public GameRoom Find(int roomId)
        {
            lock (_lock)
            { // TryGetValue 부분 숙지 !!
                GameRoom room = null;
                if (_rooms.TryGetValue(roomId, out room))
                    return room;
                return null;
            }
        }



    }
}
