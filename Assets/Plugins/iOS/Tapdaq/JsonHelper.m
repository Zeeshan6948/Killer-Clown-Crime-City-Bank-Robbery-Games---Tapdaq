#import "JsonHelper.h"

@implementation JsonHelper

+(NSString *) toJsonString:(NSDictionary *) dict {
    if([NSJSONSerialization isValidJSONObject:dict])
    {
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict
                                                       options:NSJSONWritingPrettyPrinted
                                                         error:&error];
    
        if (! jsonData) {
            NSLog(@"toJsonString: error: %@", error.localizedDescription);
            return @"{}";
        } else {
            return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        }
    }
    return @"{}";
}

+(NSDictionary *) fromJsonString:(NSString *) json {
    NSError *err = nil;
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:
                          [json dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&err];
    return dict;
}

@end
