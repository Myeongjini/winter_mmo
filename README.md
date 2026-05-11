# winter_mmo

Unity 2D 클라이언트와 C# TCP 서버로 구성된 멀티플레이 RPG 프로토타입입니다.  
클라이언트와 서버는 Google Protobuf로 정의한 패킷을 주고받으며, 서버 권한 방식으로 플레이어 입장, 이동, 스킬 사용, 스폰/디스폰 동기화를 처리합니다.

## 주요 기능

- Unity 기반 2D 타일맵 RPG 클라이언트
- C# TCP 서버 및 세션 관리
- Google Protobuf 기반 패킷 직렬화
- 플레이어 접속, 입장, 퇴장 동기화
- 플레이어 이동 패킷 처리 및 브로드캐스트
- 스킬 입력 및 스킬 패킷 브로드캐스트
- 서버 측 맵 충돌 체크
- Protobuf 코드 및 패킷 매니저 자동 생성 도구

## 기술 스택

- Client: Unity `2019.3.15f1`
- Server: C# / .NET Core `3.1`
- Protocol: Google Protobuf `3.12.3`
- Network: TCP Socket

## 프로젝트 구조

```text
winter_mmo/
├── Client/                         # Unity 클라이언트 프로젝트
│   ├── Assets/
│   │   ├── Scenes/Game.unity
│   │   ├── Scripts/
│   │   │   ├── Controllers/         # 플레이어, 몬스터, 투사체 컨트롤러
│   │   │   ├── Managers/            # 리소스, 맵, 네트워크, UI 매니저
│   │   │   ├── Packet/              # Protobuf 프로토콜 및 패킷 처리
│   │   │   └── ServerCore/          # 클라이언트용 네트워크 코어
│   │   └── Resources/               # 프리팹, 애니메이션, 맵 데이터
│   ├── Packages/
│   └── ProjectSettings/
├── Server/
│   ├── Server.sln
│   ├── Server/                      # 게임 서버
│   │   ├── Game/                    # 룸, 맵, 오브젝트 로직
│   │   ├── Packet/                  # 서버 패킷 처리
│   │   └── Session/                 # 클라이언트 세션 관리
│   ├── ServerCore/                  # TCP Listener, Session, Buffer 등 공통 네트워크 코어
│   └── PacketGenerator/             # 패킷 매니저 코드 생성기
└── Common/
    ├── MapData/                     # 공통 맵 데이터
    └── protoc-3.12.3-win64/         # protoc 및 Protocol.proto
```

## 실행 준비

### 필수 설치

- Unity Hub
- Unity `2019.3.15f1`
- .NET Core SDK `3.1`
- Visual Studio 또는 Rider

### 서버 실행

1. `Server/Server.sln`을 Visual Studio 또는 Rider로 엽니다.
2. 시작 프로젝트를 `Server`로 설정합니다.
3. 서버를 실행합니다.
4. 콘솔에 `Listening...`이 출력되면 서버가 실행 중입니다.

서버는 현재 실행 머신의 호스트 IP와 `7777` 포트로 바인딩됩니다.

### 클라이언트 실행

1. Unity Hub에서 `Client/` 폴더를 프로젝트로 엽니다.
2. `Assets/Scenes/Game.unity` 씬을 엽니다.
3. 서버가 실행 중인 상태에서 Play 버튼을 누릅니다.

클라이언트는 현재 코드 기준으로 같은 머신의 호스트 IP와 `7777` 포트에 접속합니다.

## 조작법

| 입력 | 동작 |
| --- | --- |
| `W` | 위로 이동 |
| `A` | 왼쪽으로 이동 |
| `S` | 아래로 이동 |
| `D` | 오른쪽으로 이동 |
| `Space` | 스킬 사용 |

## 패킷 생성

프로토콜 정의 파일은 다음 경로에 있습니다.

```text
Common/protoc-3.12.3-win64/bin/Protocol.proto
```

Windows 환경에서는 아래 배치 파일을 실행해 Protobuf C# 코드와 클라이언트/서버 패킷 매니저 코드를 생성할 수 있습니다.

```text
Common/protoc-3.12.3-win64/bin/GenProto.bat
```

생성 결과는 다음 위치로 복사됩니다.

- `Client/Assets/Scripts/Packet/Protocol.cs`
- `Client/Assets/Scripts/Packet/ClientPacketManager.cs`
- `Server/Server/Packet/Protocol.cs`
- `Server/Server/Packet/ServerPacketManager.cs`

## 현재 구현 상태

- 서버는 `RoomManager`를 통해 기본 룸을 생성하고 플레이어를 입장시킵니다.
- 플레이어는 접속 시 서버에서 오브젝트 ID와 초기 위치를 할당받습니다.
- 이동은 클라이언트 입력 후 `C_Move` 패킷으로 서버에 전달되고, 서버가 `S_Move`로 전체 플레이어에게 브로드캐스트합니다.
- 스킬 입력은 `C_Skill` 패킷으로 전달되며, 서버가 `S_Skill`로 브로드캐스트합니다.
- 서버 맵 로직은 이동 가능 여부와 오브젝트 점유 상태를 확인합니다.

## 참고 사항

- 이 프로젝트는 학습 및 프로토타입 목적의 MMO/RPG 네트워크 구조 실험 프로젝트입니다.
- 서버와 클라이언트가 같은 호스트 이름 기준 IP를 사용하도록 작성되어 있어, 원격 접속 테스트 시 IP 바인딩/접속 설정을 조정해야 할 수 있습니다.
- `.NET Core 3.1`은 구버전 런타임이므로 새 환경에서 실행할 경우 SDK 설치 여부를 먼저 확인해야 합니다.
