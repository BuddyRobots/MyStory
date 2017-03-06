#import <Foundation/Foundation.h>

@interface PhotoManager : NSObject
- ( void ) imageSaved: (UIImage *) image didFinishSavingWithError:( NSError *)error
          contextInfo: ( void *) contextInfo;

- (void)videoSaved:(NSString *)videoPath didFinishSavingWithError:(NSError *)error
            contextInfo:(void *)contextInfo;

@end
