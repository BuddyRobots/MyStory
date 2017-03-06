//
//  ShareRecUnity3DExtension.m
//  ShareRecGameSample
//
//  Created by vimfung on 14-11-14.
//  Copyright (c) 2014年 掌淘科技. All rights reserved.
//

#import "ShareRecUnity3DExtension.h"
#import <ShareREC/ShareREC.h>
#import <ShareREC/Extension/ShareREC+Ext.h>
#import "JSONKit.h"

#if defined (__cplusplus)
extern "C" {
#endif
    
    /**
     *	@brief	初始化ShareRec
     *
     *	@param 	appKey 	应用Key
     */
    extern void __iosShareRECRegisterApp(void *appKey);
    
    /**
     *	@brief	开始录制
     */
    extern void __iosShareRECStartRecording();
    
    /**
     *	@brief	结束录制
     *
     *  @param  observer    观察回调对象名称
     */
    extern void __iosShareRECStopRecording (void *observer);
    
    /**
     *	@brief	播放最后一个录像
     */
    extern void __iosShareRECPlayLastRecording ();
    
    /**
     *	@brief	设置码率，默认为800kbps = 800 * 1024
     *
     *	@param 	bitRate 	码率
     */
    extern void __iosShareRECSetBitRate (int bitRate);
    
    /**
     *	@brief	设置帧率
     *
     *	@param 	fps 	帧率
     */
    extern void __iosShareRECSetFPS (int fps);
    
    /**
     *	@brief	设置最短录制时间，默认4秒
     *
     *	@param 	time    时间，0表示不限制
     */
    extern void __iosShareRECSetMinimumRecordingTime(float time);
    
    /**
     *	@brief	获取最后一个录像的路径
     */
    extern const char* __iosShareRECLastRecordingPath ();
    
    /**
     *  编辑最后一个录像
     *
     *  @param title    标题
     *  @param userData 用户数据
     *  @param observer 回调对象名称
     */
    extern void __iosShareRECEditLastRecording (void *title, void *userData, void *observer);
    
    /**
     *  编辑最后一个录像
     *
     *  @param observer 回调对象名称
     */
    extern void __iosShareRECEditLastRecordingNew (void *observer);
    
    /**
     *  设置是否同步录入语音解说
     *
     *  @param syncAudioComment 同步语音解说标识，YES 表示同步录入, NO 表示不录入。
     */
    extern void __iosShareRECSetSyncAudioComment (bool syncAudioComment);
    
#if defined (__cplusplus)
}
#endif

#if defined (__cplusplus)
extern "C" {
#endif
    
    extern void UnitySendMessage(const char* obj, const char* method, const char* msg);
    
    void __iosShareRECRegisterApp(void *appKey)
    {
        NSString *appKeyStr = nil;
        if (appKey)
        {
            appKeyStr = [NSString stringWithCString:appKey encoding:NSUTF8StringEncoding];
        }
        
        [ShareREC registerApp:appKeyStr];
    }
    
    void __iosShareRECStartRecording()
    {
        [ShareREC startRecording];
    }
    
    void __iosShareRECStopRecording (void *observer)
    {
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareREC stopRecording:^(NSError *error) {
            
            NSMutableDictionary *resultDict = [NSMutableDictionary dictionaryWithDictionary:@{@"name" : @"StopRecordingFinished"}];
            if (error)
            {
                NSDictionary *errDict = @{@"code" : @(error.code), @"message" : error.localizedDescription ? error.localizedDescription : @""};
                [resultDict setObject:errDict forKey:@"error"];
            }
            
            NSString *resultStr = [resultDict JSONString];
            UnitySendMessage([observerStr UTF8String], "shareRECCallback", [resultStr UTF8String]);
            
            
            
            ////////////添加方法，获取整个录制视频的列表
            /*
            NSArray *recordings = [ShareREC currentLocalRecordings];
            SRERecording *recording = [recordings lastObject];
            NSString *path = [recording mergeAudioVideoPath];
            */
            ////////////////////////////
            
            
            //必须添加的方法
//            SRERecording *recording=[[ShareREC currentLocalRecordings]lastObject];
//            [sharerec confirmEditRecording: recording result:^(BOOL successed,NSError *error)
//             
//             NSString *Path=recording.mergeAudioVideoPath;
//             ALAssetsLibrary *library=[[ALAssetsLibrary alloc] init];
//             [library writeVideoAtPathToSavedPhotosAlbum:[NSURL fileURLWithPath: Path]
//                                         completionBlock:^(NSURL *assetURL, NSError *error)
//              {
//                  if (error)
//                  {
//                      NSLog(@"保存失败：%@",error);
//            
//                  }else
//                  {
//                      NSLog(@"保存成功");
//                  }
//              
//              
//              }
//              ];
            
//             ]
            
            
            
            
            
        }];
    }
    
    void __iosShareRECPlayLastRecording ()
    {
        [ShareREC playLastRecording];
    }
    
    void __iosShareRECSetBitRate (int bitRate)
    {
        [ShareREC setBitRate:bitRate];
    }
    
    void __iosShareRECSetFPS (int fps)
    {
        [ShareREC setFPS:fps];
    }
    
    void __iosShareRECSetMinimumRecordingTime(float time)
    {
        [ShareREC setMinimumRecordingTime:time];
    }
    
    const char* __iosShareRECLastRecordingPath ()
    {
        if ([ShareREC lastRecordingPath])
        {
            return strdup([[ShareREC lastRecordingPath] UTF8String]);
        }
        
        return strdup([@"" UTF8String]);
    }
    
    void __iosShareRECEditLastRecording (void *title, void *userData, void *observer)
    {
        NSString *titleStr = nil;
        if (title)
        {
            titleStr = [NSString stringWithCString:title encoding:NSUTF8StringEncoding];
        }
        
        NSDictionary *userDataDict = nil;
        if (userData)
        {
            NSString *userDataStr = [NSString stringWithCString:userData encoding:NSUTF8StringEncoding];
            userDataDict = [userDataStr objectFromJSONString];
        }
        
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareREC editLastRecordingWithTitle:titleStr userData:userDataDict onClose:^{
            
            NSDictionary *resultDict = @{@"name" : @"SocialClose"};
            NSString *resultStr = [resultDict JSONString];
            UnitySendMessage([observerStr UTF8String], "shareRECCallback", [resultStr UTF8String]);
            
        }];
    }
    
    void __iosShareRECEditLastRecordingNew (void *observer)
    {
        NSString *observerStr = nil;
        if (observer)
        {
            observerStr = [NSString stringWithCString:observer encoding:NSUTF8StringEncoding];
        }
        
        [ShareREC editLastRecording:^(BOOL cancelled) {
           
            NSDictionary *resultDict = @{@"name" : @"EditResult", @"cancelled" : @(cancelled)};
            NSString *resultStr = [resultDict JSONString];
            UnitySendMessage([observerStr UTF8String], "shareRECCallback", [resultStr UTF8String]);
            
        }];
    }
    
    void __iosShareRECSetSyncAudioComment (bool syncAudioComment)
    {
        [ShareREC setSyncAudioComment:syncAudioComment ? YES : NO];
    }
    
#if defined (__cplusplus)
}
#endif

@implementation ShareRecUnity3DExtension

@end
