#import "PhotoManager.h"

@implementation PhotoManager
- ( void ) imageSaved: ( UIImage *) image didFinishSavingWithError:( NSError *)error
          contextInfo: ( void *) contextInfo
{
    NSLog(@"图片保存结束");
    if (error != nil) {
        NSLog(@"图片保存有错误");
    }
}

- (void)videoSaved:(NSString *)videoPath didFinishSavingWithError:(NSError *)error
                contextInfo:(void *)contextInfo;
{
    NSLog(@"视频保存结束");
    if (error != nil) {
        NSLog(@"视频保存有错误");
    }
}





void _IOSSaveImageToPhotosAlbum(char *readAddr)
{
    NSString *strReadAddr = [NSString stringWithUTF8String:readAddr];
    UIImage *img = [UIImage imageWithContentsOfFile:strReadAddr];
//    NSLog([NSString stringWithFormat:@"w:%f, h:%f", img.size.width, img.size.height]);
    PhotoManager *instance = [PhotoManager alloc];
    UIImageWriteToSavedPhotosAlbum(img, instance,
                                   @selector(imageSaved:didFinishSavingWithError:contextInfo:), nil);
}


void _IOSSaveVideoToPhotosAlbum(char *videoPath)
{
    NSString *_nsVideoPath = [NSString stringWithUTF8String:videoPath];
    BOOL compatible = UIVideoAtPathIsCompatibleWithSavedPhotosAlbum(_nsVideoPath);
    if (compatible)
    {
        PhotoManager *instance = [PhotoManager alloc];
        UISaveVideoAtPathToSavedPhotosAlbum(_nsVideoPath, instance,
                                            @selector(videoSaved:didFinishSavingWithError:contextInfo:), nil);
    }
}



@end
