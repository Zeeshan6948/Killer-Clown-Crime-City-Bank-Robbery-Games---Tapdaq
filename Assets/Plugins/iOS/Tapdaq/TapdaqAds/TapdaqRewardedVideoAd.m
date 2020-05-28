#import "TapdaqStandardAd.h"

@implementation TapdaqRewardedVideoAd

+ (TapdaqRewardedVideoAd *)sharedInstance
{
    static dispatch_once_t once;
    static TapdaqRewardedVideoAd* sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

- (void)loadForPlacementTag:(NSString *)tag {
    [[Tapdaq sharedSession] loadRewardedVideoForPlacementTag:tag];
}

- (BOOL)isReadyForPlacementTag:(NSString *)tag {
    return [[Tapdaq sharedSession] isRewardedVideoReadyForPlacementTag:tag];
}
- (void)showForPlacementTag:(NSString *) tag withHashedUserId:(NSString *) hashedUserId {
    [[Tapdaq sharedSession] showRewardedVideoForPlacementTag:tag hashedUserId:hashedUserId];
}
@end
