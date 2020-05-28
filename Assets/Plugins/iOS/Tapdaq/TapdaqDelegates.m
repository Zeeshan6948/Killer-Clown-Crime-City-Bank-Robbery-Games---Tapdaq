#import "TapdaqDelegates.h"
#import "JsonHelper.h"

@implementation TapdaqDelegates

+ (instancetype)sharedInstance
{
    static dispatch_once_t once;
    static id sharedInstance;
    dispatch_once(&once, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

- (NSDictionary *)errorDictWithError:(NSError *)error {
    if (error != nil) {
        NSMutableDictionary *subErrorDicts = [NSMutableDictionary dictionary];
        if ([error isKindOfClass:TDError.class]) {
            for (NSString *networkName in [(TDError *)error subErrors].allKeys) {
                NSArray *errors = [(TDError *)error subErrors][networkName];
                NSMutableArray *networkErrors = NSMutableArray.array;
                
                for (NSError *networkError in errors) {
                    NSDictionary *dict = @{
                                           @"code": @(networkError.code),
                                           @"message": (networkError.localizedDescription == nil ? @"" : networkError.localizedDescription)
                                           };
                    [networkErrors addObject:dict];
                }
                subErrorDicts[networkName] = networkErrors;
            }
        }
        return @{
                 @"code": @(error.code),
                 @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription),
                 @"subErrors": subErrorDicts
                 };
    }
    return nil;
}

- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message
{
    [self send:methodName adType:adType tag:tag message:message error:nil];
}

- (void) send:(NSString *) methodName adType:(NSString *) adType tag:(NSString *) tag message: (NSString *) message error:(NSError *)error
{
    NSMutableDictionary* dict = [@{
                                   @"adType": adType,
                                   @"tag": tag,
                                   @"message": message
                                   } mutableCopy];
    if (error != nil) {
        dict[@"error"] = [self errorDictWithError:error];
    }
    [self send: methodName dictionary: dict];
}

- (void) send:(NSString *) methodName error:(NSError *)error
{
    NSDictionary * errorDict = [self errorDictWithError:error];
    [self send: methodName dictionary: errorDict != nil ? errorDict : @{}];
}

- (void) send:(NSString *) methodName dictionary:(NSDictionary *) dictionary
{
    [self send: methodName message: [JsonHelper toJsonString: dictionary]];
}

- (void) send:(NSString *) methodName message:(NSString *) message
{
    UnitySendMessage("TapdaqV1", [methodName UTF8String], [message UTF8String]);
}

#pragma mark - TapdaqDelegate

- (void)didLoadConfig
{
    [self send:@"_didLoadConfig" message:@""];
}

- (void)didFailToLoadConfigWithError:(TDError *)error
{
    [self send:@"_didFailToLoadConfig" error:error];
}

#pragma mark Banner delegate methods

- (void)didLoadBanner
{
    [self send: @"_didLoad" adType: @"BANNER" tag: @"" message: @"LOADED"];
}

- (void)didFailToLoadBannerWithError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"BANNER" tag: @"" message: @"LOAD_FAILED" error:error];
}

- (void)didClickBanner
{
    [self send: @"_didClick" adType: @"BANNER" tag: @"" message: @"CLICK"];
}

- (void)didRefreshBanner
{
    [self send: @"_didRefresh" adType: @"BANNER" tag: @"" message: @"REFRESH"];
}

#pragma mark Interstitial delegate methods

- (void)didLoadInterstitialForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didLoad" adType: @"INTERSTITIAL" tag: placementTag message: @"LOADED"];
}

- (void)didFailToLoadInterstitialForPlacementTag:(NSString *)placementTag withError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"INTERSTITIAL" tag: placementTag message: @"LOAD_FAILED" error:error];
}

- (void)willDisplayInterstitialForPlacementTag:(NSString *) placementTag
{
    [self send: @"_willDisplay" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

- (void)didDisplayInterstitialForPlacementTag: (NSString *) placementTag
{
    [self send: @"_didDisplay" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

- (void)didFailToDisplayInterstitialForPlacementTag:(NSString *)placementTag withError:(NSError *)error
{
    [self send: @"_didFailToDisplay" adType: @"INTERSTITIAL" tag: placementTag message: @"" error:error];
}

- (void)didCloseInterstitialForPlacementTag: (NSString *) placementTag
{
    [self send: @"_didClose" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

- (void)didClickInterstitialForPlacementTag: (NSString *) placementTag
{
    [self send: @"_didClick" adType: @"INTERSTITIAL" tag: placementTag message: @""];
}

#pragma mark Video delegate methods

- (void)didLoadVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didLoad" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didFailToLoadVideoForPlacementTag:(NSString *)placementTag withError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"VIDEO" tag: placementTag message: @"" error:error];
}

- (void)willDisplayVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_willDisplay" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didDisplayVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didDisplay" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didFailToDisplayVideoForPlacementTag:(NSString *)placementTag withError:(NSError *)error
{
    [self send: @"_didFailToDisplay" adType: @"VIDEO" tag: placementTag message: @"" error:error];
}

- (void)didCloseVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClose" adType: @"VIDEO" tag: placementTag message: @""];
}

- (void)didClickVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClick" adType: @"VIDEO" tag: placementTag message: @""];
}

#pragma mark Rewarded Video delegate methods

- (void)didLoadRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didLoad" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didFailToLoadRewardedVideoForPlacementTag:(NSString *)placementTag withError:(TDError *)error
{
    [self send: @"_didFailToLoad" adType: @"REWARD_AD" tag: placementTag message: @"" error:error];
}

- (void)willDisplayRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_willDisplay" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didDisplayRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didDisplay" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didFailToDisplayRewardedVideoForPlacementTag:(NSString *)placementTag withError:(NSError *)error
{
    [self send: @"_didFailToDisplay" adType: @"REWARD_AD" tag: placementTag message: @"" error:error];
}

- (void)didCloseRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClose" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)didClickRewardedVideoForPlacementTag:(NSString *)placementTag
{
    [self send: @"_didClick" adType: @"REWARD_AD" tag: placementTag message: @""];
}

- (void)rewardValidationSucceeded:(TDReward *)reward {
    [self handleDidVerifyReward:reward];
}

- (void)rewardValidationFailed:(TDReward *)reward {
    [self handleDidVerifyReward:reward];
}

- (void)handleDidVerifyReward:(TDReward *)reward {
    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
    
    dict[@"RewardName"] = reward.name;
    dict[@"RewardAmount"] = @(reward.value);
    dict[@"RewardValid"] = @(reward.isValid);
    dict[@"Tag"] = reward.tag;
    dict[@"EventId"] = reward.eventId;
    
    if (reward.customJson != nil) {
        dict[@"RewardJson"] = [JsonHelper toJsonString:reward.customJson];
    } else {
        dict[@"RewardJson"] = [JsonHelper toJsonString:[NSDictionary dictionary]];
    }
    [self send: @"_didVerify" dictionary: dict];
}

@end

