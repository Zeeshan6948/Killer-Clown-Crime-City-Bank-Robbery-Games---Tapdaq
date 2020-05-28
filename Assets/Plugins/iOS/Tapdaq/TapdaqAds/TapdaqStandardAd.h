#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>

@protocol TapdaqStandardAd

+ (instancetype) sharedInstance;
- (BOOL)isReady;
- (void)show;
- (void)load;

@optional
- (void)loadForPlacementTag:(NSString *)tag;
- (BOOL)isReadyForPlacementTag:(NSString *)tag;
- (void)showForPlacementTag:(NSString *)tag;
- (void)showForPlacementTag:(NSString *) tag withHashedUserId:(NSString *) hashedUserId;

@end

@interface TapdaqVideoAd : NSObject<TapdaqStandardAd>
@end

@interface TapdaqRewardedVideoAd : NSObject<TapdaqStandardAd>
@end

@interface TapdaqInterstitialAd : NSObject<TapdaqStandardAd>
@end

@interface TapdaqBannerAd : NSObject<TapdaqStandardAd>

- (void)loadForSize:(NSString *)sizeStr;
- (void)hide;
- (void)show:(const char *)position;

@end
