import os
from PIL import Image

# 遍历文件夹中的所有 PNG 文件，压缩并降低分辨率
def compress_and_resize_pngs_in_directory(directory, output_directory, scale_factor=0.5, quality=85):
    # 如果输出目录不存在，创建它
    if not os.path.exists(output_directory):
        os.makedirs(output_directory)

    # 使用 os.walk 递归遍历目录及其子目录
    for root, dirs, files in os.walk(directory):
        # 创建对应的子目录结构
        relative_path = os.path.relpath(root, directory)
        output_subdirectory = os.path.join(output_directory, relative_path)
        if not os.path.exists(output_subdirectory):
            os.makedirs(output_subdirectory)

        # 遍历文件
        for filename in files:
            if filename.endswith('.png'):
                file_path = os.path.join(root, filename)
                
                # 打开 PNG 文件
                with Image.open(file_path) as img:
                    # 获取原始尺寸
                    original_width, original_height = img.size

                    # 计算新的尺寸
                    new_width = int(original_width * scale_factor)
                    new_height = int(original_height * scale_factor)

                    # 调整分辨率并使用 LANCZOS 进行高质量缩放
                    resized_img = img.resize((new_width, new_height), Image.LANCZOS)

                    # 压缩并保存 PNG
                    output_file = os.path.join(output_subdirectory, filename)
                    resized_img.save(output_file, optimize=True, quality=quality)
                    print(f"Resized and compressed {file_path} and saved to {output_file}")

# 设置输入目录和输出目录
input_directory = r'E:\BrackeysGameJam\demo\Assets\AtmosphericHouse\Textures\Worn'  # 替换为你的实际目录
output_directory = r'E:\BrackeysGameJam\demo\Assets\AtmosphericHouse\Textures\Compressed'  # 替换为保存压缩后的文件的目录

# 压缩 PNG 文件并减少分辨率，scale_factor 代表缩小比例
compress_and_resize_pngs_in_directory(input_directory, output_directory, scale_factor=0.5, quality=85)
