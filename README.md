# Twilight Forest

Twilight Forest là game 2D phiêu lưu - hành động được xây dựng bằng Unity. Người chơi điều khiển nhân vật khám phá màn chơi, chiến đấu với kẻ địch, hoàn thành nhiệm vụ để mở cổng và chuyển sang khu vực tiếp theo.

Game tập trung vào vòng lặp gameplay cơ bản: di chuyển, tấn công, thu thập vật phẩm, quản lý máu/stamina, hoàn thành nhiệm vụ, Game Over và chơi lại.

## Screenshot / Gameplay

> Chưa đính kèm ảnh trong repository. Khi nộp bài, có thể chụp 2-3 ảnh màn hình gameplay và đặt vào thư mục `Docs/Images/`.

Gợi ý ảnh nên chụp:

1. `Docs/Images/main-menu.png` - màn hình menu chính, có phần chọn skin.
2. `Docs/Images/gameplay.png` - nhân vật đang chiến đấu với quái.
3. `Docs/Images/objective-portal.png` - UI nhiệm vụ và portal mở sau khi hoàn thành.
4. `Docs/Images/game-over.png` - màn hình Game Over.

Sau khi có ảnh, thêm vào README theo mẫu:

```md
![Main Menu](Docs/Images/main-menu.png)
![Gameplay](Docs/Images/gameplay.png)
![Game Over](Docs/Images/game-over.png)
```

## Tính Năng Chính

- Di chuyển nhân vật 4 hướng bằng bàn phím.
- Tấn công kẻ địch và xử lý hiệu ứng khi enemy bị đánh.
- Hệ thống máu, stamina và cảnh báo máu thấp.
- Nhặt vật phẩm như vàng, hồi máu và hồi stamina.
- Nhiệm vụ mở cổng: người chơi cần hoàn thành mục tiêu trước khi chuyển màn.
- Hiển thị tiến độ nhiệm vụ trên giao diện.
- Pause Menu: tiếp tục chơi hoặc quay về menu chính.
- Game Over: chơi lại màn hiện tại hoặc quay về menu chính.
- Âm thanh: nhạc nền, âm thanh đánh quái, nhận sát thương, chuyển màn và Game Over.
- Chọn skin nhân vật và lưu lựa chọn bằng `PlayerPrefs`.

## Công Nghệ Sử Dụng

- Unity `6000.3.10f1`
- C#
- Unity 2D
- Rigidbody2D và Collider2D
- Unity New Input System
- TextMeshPro
- Unity UI Canvas
- Git / GitHub

## Hướng Dẫn Cài Đặt Và Chạy

### Cách 1: Chạy bằng Unity Editor

1. Clone repository:

   ```bash
   git clone https://github.com/0Yaam/Twilight-Forest.git
   ```

2. Mở Unity Hub.
3. Chọn **Add project from disk**.
4. Chọn thư mục project vừa clone.
5. Mở project bằng Unity `6000.3.10f1` hoặc phiên bản Unity 6 tương thích.
6. Mở scene:

   ```text
   Assets/Scenes/MainMenu.unity
   ```

7. Bấm **Play** trong Unity Editor để chạy game.

### Cách 2: Chạy bản build có sẵn

Nếu repository có thư mục `Builds/`, có thể chạy trực tiếp bản build:

- Windows:

  ```text
  Builds/window/Twilight-Forest.exe
  ```

- macOS:

  ```text
  Builds/macos/macos.app
  ```

Lưu ý: Trên macOS, nếu bị chặn do ứng dụng tải từ Internet, vào **System Settings > Privacy & Security** để cho phép mở ứng dụng.

## Điều Khiển Cơ Bản

| Phím / Thao tác | Chức năng |
|---|---|
| `W A S D` hoặc phím di chuyển | Di chuyển nhân vật |
| Chuột trái / phím tấn công đã cấu hình | Tấn công |
| `Esc` | Mở / đóng Pause Menu |
| Nút UI | Chọn skin, Play, Resume, Restart hoặc Main Menu |

## Thành Viên Và Phân Công

| Thành viên | Phân công |
|---|---|
| Thành viên 1 | Lập trình gameplay, di chuyển, chiến đấu, nhiệm vụ mở cổng |
| Thành viên 2 | Thiết kế UI, Main Menu, Pause Menu, Game Over |
| Thành viên 3 | Âm thanh, kiểm thử, build game và viết báo cáo |

> Cập nhật lại tên thành viên thật trước khi nộp bài.

## Yêu Cầu Hệ Thống

### Khi chạy bằng Unity Editor

- Hệ điều hành: Windows 10/11 hoặc macOS phiên bản Unity hỗ trợ.
- Unity: `6000.3.10f1` hoặc Unity 6 tương thích.
- RAM khuyến nghị: 8 GB trở lên.
- Dung lượng trống: tối thiểu 2 GB.
- Git để clone project.

### Khi chạy bản build

- Windows 10/11 64-bit hoặc macOS.
- RAM khuyến nghị: 4 GB trở lên.
- GPU hỗ trợ đồ họa 2D cơ bản.
- Dung lượng trống: tối thiểu 500 MB.

## Ghi Chú

Project hiện được thiết kế theo hướng single-player. Một số hệ thống như Player, Camera, UI và Enemy AI đang phục vụ cho một người chơi, chưa hỗ trợ multiplayer online.
